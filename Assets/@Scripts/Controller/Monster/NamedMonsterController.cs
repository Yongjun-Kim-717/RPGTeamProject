using System;
using Unity.VisualScripting;
using UnityEngine;

public class NamedMonsterController : MonsterController
{
    public static Action s_OnNamedMonsterDie;
    public static Action<float, float> s_OnNamedMonsterGetDamage;
    public static Action<float, float> s_OnNamedMonsterSet;

    [SerializeField] private float _closeAttackLimit = 4f;
    [SerializeField] private bool _isMoveToOrigin;
    [SerializeField] private Vector3 _originPos;
    [SerializeField] private SkillData _bulletSkillData;
    [SerializeField] private WeaponColliderController _weapon;

    float _coolTime = 0.0f;

    private MoveRangeController _moveRangeController;

    private ActiveSkill _bulletSkill;

    public override void Initialize()
    {
        SetOriginPos();
        base.Initialize();
        GameObject moveRange = new GameObject("MoveRange");
        moveRange.transform.parent = this.gameObject.transform;
        _moveRangeController = moveRange.GetOrAddComponent<MoveRangeController>();
        _moveRangeController.Intiailize(_runtimeData.MoveRange);
        _weapon.SetColliderInfo(_runtimeData.Atk);
    }
    protected override void OnEnable()
    {
        SetOriginPos();
        base.OnEnable();
        _isMoveToOrigin = false;
        _moveRangeController.OnMoveToTarget += OnMoveToTarget;
        _moveRangeController.OnMoveToOrigin += StopMove;
        _attackRangeController.OffAttack += EndAttack;
        s_OnNamedMonsterSet?.Invoke(_runtimeData.CurrentHP, _runtimeData.MaxHP);
    }

    private void Start()
    {
        _bulletSkill = Instantiate(ObjectManager.Instance.MonsterSkillResourceList[_bulletSkillData.SkillName]).
            GetComponent<ActiveSkill>();
        _bulletSkill.Initialize(_monsterData);
        _bulletSkill.gameObject.SetActive(false);
    }


    private void Update()
    {
        RotateNamedMonster();
        CheckMoveToOrigin();
    }

    void SetOriginPos()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 10f);
        _originPos = hit.point;
    }

    void RotateNamedMonster()
    {
        if (!_isMoveToOrigin)
        {
            Vector3 dir = _target.transform.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    //원래 자리로 갈지 정하는 함수
    void CheckMoveToOrigin()
    {
        //원래 자리로 가야 하면
        if (_isMoveToOrigin)
        {
            //원래 자리로 움직인다.
            MoveToTarget(_originPos);
            if ((transform.position - _originPos).sqrMagnitude < 0.1f ||
                Vector3.Distance(transform.position, _target.transform.position) < _runtimeData.MoveRange)
            {
                StopMove();
            }
        }

    }

    //moveRange 범위 안에 있을 때 실행되는 함수
    //5~8범위 내에서는 이동
    void OnMoveToTarget()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        if (distance > _closeAttackLimit && distance <= _runtimeData.MoveRange)
            MoveToTarget(_target.transform.position);
        else
            StopMove();
    }

    void StopMove()
    {
        _isMoveToOrigin = false;
        _animator.SetFloat(Define.WalkSpeed, 0);
    }

    //공격 범위 내에 있으면 계속 호출될 함수
    public override void Attack()
    {
        if (_isMoveToOrigin)
            StopMove();

        float distance = Vector3.Distance(transform.position, _target.transform.position);

        //0~3범위 내에서는 이동하지 않고 공격
        if (distance < _closeAttackLimit)
        {
            ActivateCloseAttack();
        }
        //타겟과의 거리가 MoveRange 범위 밖에 있고 AttackRange 안에 있을 때에는 공격
        else if (distance > _runtimeData.MoveRange && distance < _runtimeData.AttackRange)
        {
            _coolTime += Time.deltaTime;
            if (_coolTime > 3f)
            {
                _coolTime = 0;
                ActivateLongAttack(distance);
            }
        }
    }

    void ActivateCloseAttack()
    {
        if (!_target.GetComponent<Animator>().GetBool(Define.IsDead))
        {
            _animator.SetTrigger(Define.CloseAttack);
        }
    }

    //원거리 공격 활성화
    void ActivateLongAttack(float distance)
    {
        if (!_target.GetComponent<Animator>().GetBool(Define.IsDead))
        {
            _animator.SetTrigger(Define.LongAttack);
            // Offset = (0,0,3)
            _bulletSkill.ActivateSkill(transform.position + Vector3.up * 2f);
        }
    }


    //원거리 공격 범위를 벗어나면 실행되는 함수
    public override void EndAttack()
    {
        //현재 위치가 원위치라면 Idle
        if (transform.position == _originPos)
            _animator.SetTrigger(Define.Idle);
        //현재 위치가 원위치가 아니라면 원위치로 이동
        else
            _isMoveToOrigin = true;
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        s_OnNamedMonsterGetDamage?.Invoke(_runtimeData.CurrentHP, _runtimeData.MaxHP);
    }

    public override void Die()
    {
        base.Die();
        s_OnNamedMonsterDie?.Invoke();
    }
}