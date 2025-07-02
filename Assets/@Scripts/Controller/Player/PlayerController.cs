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
    public Action<int> OnJourneyRankChanged;
    public Action OnPlayerCrossed;

    PlayerStatus _runtimeData;
    Vector3 _direction;
    float _mp;
    float _hp;
    
    float _shortestSkillDistance;       //자동일 때, 이동 멈추는 범위

    bool _isSwifting;                   //질풍참 사용 여부
    bool _isKeyBoard;
    bool _isJoyStick;

    public Action OnAutoOff;
    public Action OnAutoDungeonChallenge;

    [SerializeField] PlayerData _playerData;
    [SerializeField] PlayerInventoryData _playerInventoryData;
    [SerializeField] Inventory _inventory;

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
    public Inventory Inventory{ get { return _inventory; } }
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

                OnJourneyRankChanged?.Invoke(_playerData.JourneyRankData.Index);
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
        SkillInventoryOnOff();
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

    public void GainJourneyExp(Define.JourneyType type)
    {
        float journeyExp = 1;
        switch (type)
        {
            case Define.JourneyType.Default:
                break;
            case Define.JourneyType.Explore:
                journeyExp = 10;
                break;
            case Define.JourneyType.Dungeon:
                journeyExp = 100;
                break;
            case Define.JourneyType.Treasure:
                journeyExp = 50;
                break;
        }
        journeyExp *= _playerData.JourneyRankData.Index;
        OnJourneyExpChanged?.Invoke(journeyExp);
        JourneyExp += journeyExp;

    }

    public void Move()
    {
        //던전에 들어가지 않았을 때 플레이어의 위치를 이동시킴
        if (!FieldManager.Instance.DungeonController.IsDungeonExist && transform.position.z >= 113.2)
        {
            Vector3 pos = transform.position;
            pos.z = 5;
            transform.position = pos;

            //구역 통과 시 여정 경험치? 증가
            GainJourneyExp(Define.JourneyType.Explore);

            OnPlayerCrossed?.Invoke();
        }

        // 자동 모드일 때
        if (!_isSwifting && PlayerManager.Instance.IsAuto)
        {
            // 던전 클리어해서 포탈 향해 가는 상황 / target==portal or null 일 때
            if (PlayerManager.Instance.IsAutoMoving)
            {
                if (_target == null)
                {
                    SetTarget();
                    // target 찾으면 autoMoving 스탑
                    if (_target?.CompareTag(Define.MonsterTag) != null)
                    {
                        PlayerManager.Instance.IsAutoMoving = false;
                    }
                }
                else if (!MoveToTarget(0.5f))
                {
                    // 포탈에 닿으면 IsAutoMoving false시키고 target 초기화
                    PlayerManager.Instance.IsAutoMoving = false;
                    _target = null;
                    return;
                }
            }
            else
            {
                // 타겟 없으면
                if (_target == null || !_target.gameObject.activeSelf)
                {
                    // 타겟 찾고
                    SetTarget();
                    // 찾았는데도 없으면 다음 스테이지 자동 이동?
                    if (PlayerManager.Instance.IsAutoMoving)
                    {
                        return;
                    }
                    Debug.Log($"Current Target: {_target.name}");
                }
                MoveToTarget(_shortestSkillDistance);
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
        //타겟과 거리가 가까워지면 정지
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

            _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _playerData.Speed * Time.fixedDeltaTime);

            _animator.SetFloat(Define.Speed, _direction.magnitude);
            //타겟 바라보게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _playerData.Speed * Time.deltaTime);
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
        _target = Util.GetNearestTarget(transform.position, _shortestSkillDistance)?.transform;
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
            }
        }
    }

    void SkillInventoryOnOff()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PopupUIManager.Instance.ActivateSkillInventoryPanel();
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

    #endregion

    #region IDamageable Methods
    // * 방어력 적용 데미지 계산 메서드
    public void GetDamage(float damage)
    {
        float finalDamage = CalculateFinalDamage(damage, _runtimeData.Def);
        HP -= finalDamage;
        DamageTextEvent.Invoke(Util.GetDamageTextPosition(gameObject.GetComponent<Collider>()), finalDamage, false);
        //Debug.Log($"Damaged: {finalDamage}, Current Player HP: {_hp}");
        //if (_runtimeData.HP <= 0)
        //    Die();
    }

    public float CalculateFinalDamage(float damage, float def)
    {
        return damage * (1 - def / Define.MaxDef);
    }
    #endregion
}