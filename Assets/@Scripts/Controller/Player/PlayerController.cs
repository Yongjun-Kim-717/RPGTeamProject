using System;
using System.Collections;
using UnityEngine;

public struct PlayerStatus
{
    public float Atk;
    public float Def;
    public float Damage;
    public float HP;
    public float HPRecoveryPerSec;
    public float CoolTimeDecrease;
    public float JourneyExp;
    public float Speed;

    public PlayerStatus(PlayerData playerData)
    {
        Atk = playerData.Atk;
        Def = playerData.Def;
        Damage = playerData.Damage;
        HP = playerData.HP;
        HPRecoveryPerSec = playerData.HPRecoveryPerSec;
        CoolTimeDecrease = playerData.CoolTimeDecrease;
        JourneyExp = playerData.JourneyExp;
        Speed = playerData.Speed;
    }

    public float GetCoolTimeDecrease()
    {
        return CoolTimeDecrease;
    }

}

public class PlayerController : MonoBehaviour, IDamageable
{
    Animator _animator;
    Rigidbody _rigidbody;
    [SerializeField] Transform _target;

    public Action<float, float> OnHPValueChanged;
    public Action<float, float> OnMPValueChanged;
    public Action<float> OnJourneyExpChanged;
    public Action OnPlayerCrossed;
    public Action OnPlayerDead;

    PlayerStatus _runtimeData;
    Vector3 _direction;
    float _mp;
    float _hp;

    float _shortestSkillDistance;       //자동일 때, 이동 멈추는 범위

    float _atkBuff = 0;                 //던전 클리어 실패 시 주어질 버프
    float _hpBuff = 0;

    bool _isSwifting;                   //질풍참 사용 여부
    bool _isKeyBoard;
    bool _isJoyStick;

    public Action OnAutoOff;
    public Action OnAutoDungeonChallenge;

    [SerializeField] PlayerData _playerData;
    [SerializeField] PlayerInventoryData _playerInventoryData;
    private Inventory _inventory;

    #region Properties

    public PlayerInventoryData PlayerInventoryData { get { return _playerInventoryData; } }

    public bool IsKeyBoard
    {
        get { return _isKeyBoard; }
        set
        {
            // 자동 이동 중인데 키보드 입력할 경우 자동 이동 종료
            if (PlayerManager.Instance.IsAuto && value)
            {
                PlayerManager.Instance.IsAuto = false;
                PlayerManager.Instance.IsAutoMoving = false;
                OnAutoOff?.Invoke();
            }
            _isKeyBoard = value;
        }
    }
    public bool IsJoyStick
    {
        get { return _isJoyStick; }
        set
        {
            // 자동 이동 중인데 조이스틱 입력할 경우 자동 이동 종료
            if (PlayerManager.Instance.IsAuto && value)
            {
                PlayerManager.Instance.IsAuto = false;
                PlayerManager.Instance.IsAutoMoving = false;
                OnAutoOff?.Invoke();
            }
            _isJoyStick = value;
        }
    }
    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }

    // 데이터는 getter만 되도록?
    public PlayerData PlayerData { get { return _playerData; } }
    public Inventory Inventory { get { return _inventory; } }
    public PlayerStatus PlayerStatus { get { return _runtimeData; } }

    public float HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
            OnHPValueChanged?.Invoke(_hp, _playerData.HP);
        }
    }

    public float JourneyExp
    {
        get { return _playerData.JourneyExp; }
        set
        {
            _playerData.JourneyExp = value;
            //해당 레벨의 맥스 값보다 현재 메달 값이 높으면 
            if (_playerData.JourneyRankData.MaxJourneyExp <= _playerData.JourneyExp)
            {
                //현재 메달 변경
                _playerData.JourneyRankData =
                    ObjectManager.Instance.JourneyRankResourceList[(_playerData.JourneyRankData.Index + 1).ToString()];
            }
        }
    }
    #endregion

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        Recover();
        //SkillInventoryOnOff();
    }

    private void FixedUpdate()
    {
        SetMoveDirectionByKeyBoard();
        Move();
    }

    void Initialize()
    {
        _hp = _playerData.HP;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _runtimeData = new PlayerStatus(_playerData);
        _playerData.JourneyRankData =
            ObjectManager.Instance.JourneyRankResourceList[_playerData.JourneyRankData.Index.ToString()];
        _inventory = new Inventory(_playerInventoryData);
    }

    #region Player Moving

    public void GainJourneyExp(int amount = default)
    {
        float journeyExp = 5; //기본 증가량
        
        //구역을 지날 때에는 해당 메달의 랭크대로 여정의 증표 증가
        if (amount == default)
            journeyExp *= _playerData.JourneyRankData.Index;
        //그냥 일반 획득이라면 스테이지대로 여정의 증표 증가
        else
            journeyExp *= amount;

        OnJourneyExpChanged?.Invoke(journeyExp);
        JourneyExp += journeyExp;
        Debug.Log("여정의 증표 획득 " + journeyExp);
    }

    public void Move()
    {
        if (PlayerManager.Instance.IsDead)
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _animator.SetFloat(Define.Speed, 0);
            return;
        }
        //던전에 들어가지 않았을 때 플레이어의 위치를 이동시킴
        if (!FieldManager.Instance.DungeonController.IsDungeonExist && transform.position.z >= 113.2)
        {
            Vector3 pos = transform.position;
            pos.z = 5;
            transform.position = pos;

            //구역 통과 시 여정 경험치? 증가
            GainJourneyExp();

            OnPlayerCrossed?.Invoke();
        }

        // 자동 모드일 때
        if (!_isSwifting && PlayerManager.Instance.IsAuto)
        {
            // 타겟 생기면 IsAutoMoving=false
            if (PlayerManager.Instance.IsAutoMoving)
            {
                SetTarget();
                if (_target == null)
                {
                    MoveAlongRoad();
                }
            }
            else
            {
                if (_target == null)
                {
                    SetTarget();
                    if (_target == null)
                    {
                        MoveAlongRoad();
                    }
                }
                else
                {
                    // 거리가 아니라 trigger 발생 시 IsAutoMoving 관리로 해야할듯
                    if (!MoveToTarget(0.5f))
                    {
                        // 오브젝트 접촉 후엔 다시 제갈길 가는 거로
                        PlayerManager.Instance.IsAutoMoving = true;
                    }
                }
            }
        }
        // 수동 모드일 때
        else
        {
            if (!_animator.GetBool(Define.IsAttacking) && _direction != Vector3.zero)
            {
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _playerData.Speed * 1 * Time.fixedDeltaTime);

                _animator.SetFloat(Define.Speed, _direction.magnitude);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _playerData.Speed * Time.deltaTime);
            }
            else
            {
                _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
                _animator.SetFloat(Define.Speed, 0);
            }
        }
        ClampPosition();
    }

    // target과의 거리가 distance 이하가 될 때까지 움직임
    bool MoveToTarget(float distance)
    {
        //타겟과 거리가 distance 이하로 되면 정지
        Vector3 targetPos = _target.position;
        targetPos.y = 0;
        Vector3 playerPos = transform.position;
        playerPos.y = 0;
        if (Vector3.Distance(targetPos, playerPos) <= distance)
        {
            _direction = Vector3.zero;
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);

            _animator.SetFloat(Define.Speed, 0);
            return false;
        }
        else
        {
            if (PlayerManager.Instance.IsAutoMoving && Mathf.Abs(_rigidbody.position.x) > 0.1f)
            {
                _direction = new Vector3(_target.position.x - transform.position.x, 0, 1);
            }
            else
            {
                _direction = _target.position - transform.position;
                _direction.y = 0;
            }
            // 공격 모션 중이지 않을 때 이동
            if (!_animator.GetBool(Define.IsAttacking))
            {
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _playerData.Speed * Time.fixedDeltaTime);

                _animator.SetFloat(Define.Speed, _direction.magnitude);
                //타겟 바라보게 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _playerData.Speed * Time.deltaTime);
            }

            return true;
        }
    }

    // 가운데 길 따라 이동
    void MoveAlongRoad()
    {
        if (Mathf.Abs(_rigidbody.position.x) > 0.1f)
        {
            _direction = new Vector3(-_rigidbody.position.x, 0, 1).normalized;
        }
        else
        {
            _direction = Vector3.forward;
        }
        _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _playerData.Speed * Time.fixedDeltaTime);
        _animator.SetFloat(Define.Speed, _direction.magnitude);
        //타겟 바라보게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _playerData.Speed * Time.deltaTime);
    }

    void ClampPosition()
    {
        Vector3 newPos = _rigidbody.position;
        newPos.x = Mathf.Clamp(newPos.x, -23, 23);
        newPos.z = Mathf.Clamp(newPos.z, -100, 300);
        _rigidbody.position = newPos;
    }

    public void Attack()
    {
        if (!_animator.GetBool(Define.IsAttacking) && Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger(Define.Attack);
        }
    }

    public void Rotate(Vector3 direction)
    {
        direction.y = transform.forward.y;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void SetMoveDirectionByKeyBoard()
    {
        // 조이스틱 이동 중엔 키보드 이동 불가
        if (_isJoyStick)
            return;
        Vector3 movement;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            IsKeyBoard = true;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            movement = new Vector3(h, 0, v);
        }
        else
        {
            IsKeyBoard = false;
            movement = Vector3.zero;
        }
        _direction = movement;
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _direction = dir;
    }
    #endregion

    #region Player Utility
    void Recover()
    {
        HP += _runtimeData.HPRecoveryPerSec * Time.deltaTime;
        if (_hp > _runtimeData.HP)
        {
            _hp = _runtimeData.HP;
        }
    }

    void SetTarget()
    {
        //switch(FieldManager.Instance.CurrentEventType)
        //{
        //    case Define.JourneyEventType.Dungeon:

        //        break;

        //    case Define.JourneyEventType.Merchant:

        //        break;

        //    case Define.JourneyEventType.TreasureBox:

        //        break;

        //    case Define.JourneyEventType.OtherObject:

        //        break;
        //}
        // 던전인 경우, 몬스터 찾기
        if (FieldManager.Instance.CurrentEventType == Define.JourneyType.Dungeon)
        {
            // 우선 최단거리 기준으로 찾아보고
            _target = Util.GetNearestTarget(transform.position, _shortestSkillDistance)?.transform;
            // 없으면 
            if (_target == null || !_target.gameObject.activeSelf)
            {

                //쵸비상 몬스터 풀 어케 가져옴
                //stage info에서 현재 스테이지의 몬스터 정보를 받아와서 이름으로
                _target = Util.GetNearestTarget(transform.position, 100f)?.transform;
                if (_target == null)
                {
                    Debug.Log("No target on field!!!");
                    PlayerManager.Instance.IsAutoMoving = true;
                    _target = FindAnyObjectByType<DungeonPortalController>()?.transform;
                    if (_target != null)
                    {
                        PlayerManager.Instance.IsAutoMoving = false;
                    }
                }
                else
                {
                    PlayerManager.Instance.IsAutoMoving = false;
                }
            }
            else
            {
                PlayerManager.Instance.IsAutoMoving = false;
            }
        }
        else
        // 던전이 아닌 경우, 필드 오브젝트 찾기
        {
            _target = GameObject.FindGameObjectWithTag(Define.FieldObjectTag)?.transform;

            if (_target != null)
            {
                if (FieldManager.Instance.CurrentEventType == Define.JourneyType.TreasureBox)
                {
                    if (_target.GetComponent<Animator>().GetBool(Define.Open))
                    {
                        PlayerManager.Instance.IsAutoMoving = true;
                        _target = null;
                    }
                    else
                    {
                        PlayerManager.Instance.IsAutoMoving = false;
                    }
                }
                else if (FieldManager.Instance.CurrentEventType == Define.JourneyType.Merchant)
                {
                    // TBD
                }
                else if (FieldManager.Instance.CurrentEventType == Define.JourneyType.OtherObject)
                {
                    // TBD
                }
            }
        }
    }

    public void SetAuto(bool flag)
    {
        PlayerManager.Instance.IsAuto = flag;
        _direction = Vector3.zero;
    }

    public void SetShortestSkillDistance(float distance)
    {
        _shortestSkillDistance = distance;
    }

    void SetPlayerCollision(bool flag)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerTag), LayerMask.NameToLayer(Define.MonsterTag), !flag);
    }

    IEnumerator CoSetPlayerCollision(float duration)
    {
        _isSwifting = true;
        SetPlayerCollision(false);
        yield return new WaitForSeconds(duration);
        SetPlayerCollision(true);
        _isSwifting = false;
    }

    public void ProcessPlayerCollision(float duration)
    {
        StartCoroutine(CoSetPlayerCollision(duration));
    }

    public void OnOffStatusUpgrade(Define.StatusType status, float amount)
    {
        switch (status)
        {
            case Define.StatusType.Atk:
                _runtimeData.Atk += amount;
                break;

            case Define.StatusType.Def:
                _runtimeData.Def += amount;
                break;

            case Define.StatusType.Damage:
                _runtimeData.Damage += amount;
                break;

            case Define.StatusType.HP:
                _runtimeData.HP += amount;
                break;

            case Define.StatusType.HPRecoveryPerSec:
                _runtimeData.HPRecoveryPerSec += amount;
                break;

            case Define.StatusType.CoolTimeDecrease:
                _runtimeData.CoolTimeDecrease += amount;
                //Debug.Log($"After cooltime reduction changed: {_runtimeData.CoolTimeDecrease}%");
                break;

            default:
                break;
        }
    }

    void Die()
    {
        _animator.SetInteger(Define.DieType, UnityEngine.Random.Range(0, 2));
        PlayerManager.Instance.IsDead = true;
        _animator.SetTrigger(Define.Die);
        Invoke("Revive", 2f);
    }

    void Revive()
    {
        HP = _playerData.HP;
        OnPlayerDead?.Invoke();
        PlayerManager.Instance.IsDead = false;
        _animator.SetBool(Define.IsDead, false);
    }

    #endregion

    public void SetPlayerBuff()
    {
        if (_atkBuff == 0)
            _atkBuff = _runtimeData.Atk / 10;
        if (_hpBuff == 0)
            _hpBuff = _runtimeData.HP / 10;
            
        OnOffStatusUpgrade(Define.StatusType.Atk, _atkBuff);
        OnOffStatusUpgrade(Define.StatusType.HP, _hpBuff);
    }

    public void RemovePlayerBuff(int buffCount)
    {
        for(int i = 0; i < buffCount; i++)
        {
            OnOffStatusUpgrade(Define.StatusType.Atk, -_atkBuff);
            OnOffStatusUpgrade(Define.StatusType.HP, -_hpBuff);
        }
        _atkBuff = 0;
        _hpBuff = 0;
    }

    #region IDamageable Methods
    // * 방어력 적용 데미지 계산 메서드
    public void GetDamage(float damage)
    {
        float finalDamage = CalculateFinalDamage(damage, _runtimeData.Def);
        HP -= finalDamage;
        DamageTextEvent.Invoke(Util.GetDamageTextPosition(gameObject.GetComponent<Collider>()), finalDamage, false);
        Debug.Log($"Damaged: {finalDamage}, Current Player HP: {_hp}");
        if (HP <= 0)
            Die();
    }

    public float CalculateFinalDamage(float damage, float def)
    {
        return damage * (1 - def / Define.MaxDef);
    }
    #endregion
}