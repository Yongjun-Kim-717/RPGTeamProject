using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class NamedMonsterController : MonsterController
{
    [SerializeField] private float _closeAttackLimit = 5f;
    [SerializeField] private bool _isMoveToOrigin;
    [SerializeField] private Vector3 _originPos;
    [SerializeField] private SkillData _explosionSkillData;
    [SerializeField] private SkillData _bulletSkillData;
    [SerializeField] private GameObject _bulletSkillPrefab;
    float _coolTime = 0.0f;

    private MoveRangeController _moveRangeController;

    private RigidbodyTargetSkill _skill;


    private void Awake()
    {
        //_playableDirector = GetComponent<PlayableDirector>();
        //_playableDirector.Play();
        _originPos = transform.position;
        base.Initialize();
        GameObject moveRange = new GameObject("MoveRange");
        moveRange.transform.parent = this.gameObject.transform;
        _moveRangeController = moveRange.GetOrAddComponent<MoveRangeController>();
        _moveRangeController.Intiailize(_runtimeData.MoveRange);

        _skill = Instantiate(_bulletSkillPrefab).GetComponent<RigidbodyTargetSkill>();
        _skill.Initialize();
        _skill.gameObject.SetActive(false);

        //오브젝트 매니저에서 가져올 예정 -> 수정 필요
    }

    private void OnEnable()
    {
        _isMoveToOrigin = false;
        _moveRangeController.OnMoveToTarget += MoveToTarget;
        _moveRangeController.OnMoveToOrigin += MoveToOrigin;
        _attackRangeController.OffAttack += EndAttack;
    }

    private void Update()
    {
        RotateNamedMonster();
        CheckMoveToOrigin();
       // Debug.Log($"네임드 몬스터와 플레이어와의 거리 : {Vector3.Distance(transform.position, _target.transform.position)}");
    }

    void RotateNamedMonster()
    {
        if (!_isMoveToOrigin)
        {
            Vector3 dir = _target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    //원래 자리로 갈지 정하는 함수
    void CheckMoveToOrigin()
    {
        if (_isMoveToOrigin)
        {
            MoveToTarget(_originPos);
            if ((transform.position - _originPos).sqrMagnitude < 0.1)
            {
                _isMoveToOrigin = false;
                _animator.SetFloat(Define.WalkSpeed, 0);
                transform.rotation = Quaternion.LookRotation(-Vector3.forward);
            }
            else if (Vector3.Distance(transform.position, _target.transform.position) < _runtimeData.MoveRange)
            {
                _isMoveToOrigin = false;
            }
        }

    }

    //moveRange 범위 안에 있을 때 실행되는 함수
    //5~8범위 내에서는 이동
    void MoveToTarget()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        if(distance > _closeAttackLimit && distance <= _runtimeData.MoveRange)
        {
            MoveToTarget(_target.transform.position);
        }
        else
        {
            _animator.SetFloat(Define.WalkSpeed, 0);
        }
    }

    //moveRange 범위 밖에 있을 때 실행되는 함수
    void MoveToOrigin()
    {
        if(_bulletSkillPrefab.activeSelf)
            DeactiveLongAttack();
        _isMoveToOrigin = true;
    }

    //공격 범위 내에 있으면 계속 호출될 함수
    public override void Attack()
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);

        if (!_isMoveToOrigin)
        {

            //0~3범위 내에서는 이동하지 않고 공격
            if (distance < _closeAttackLimit)
            {
                _animator.SetTrigger(Define.CloseAttack);
                DeactiveLongAttack();
            }
            
            else if (distance > _runtimeData.MoveRange && distance < _runtimeData.AttackRange)
            {
               _coolTime += Time.deltaTime;
               if (_coolTime > 3f)
               {
                    _coolTime = 0;
                    ActiveLongAttack(distance);
               }
            }
        }
    }

    //원거리 공격 활성화
    void ActiveLongAttack(float distance)
    {
        _bulletSkillData.force = distance + 10;
        _animator.SetTrigger(Define.LongAttack);
        _skill.ActivateSkill(_target.transform, transform.position + Vector3.up * 2);
    }

    //원거리 공격 비활성화
    void DeactiveLongAttack()
    {

    }

    //원거리 공격 범위를 벗어나면 공격 종료
    public override void EndAttack()
    {
        _animator.SetTrigger(Define.Idle);
    }
}
