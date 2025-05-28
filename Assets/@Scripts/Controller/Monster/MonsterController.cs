using UnityEngine;

// * Monster Status 구조체
//- Scriptable Object의 런타임 복사용으로 활용
public struct MonsterStatus
{
    public string Name;                 // 이름
    public string Description;          // 설명
    public float Atk;                   // 공격력
    public float Def;                   // 방어력
    public float HP;                    // 체력
    public float Speed;                 // 이동 속도
    public float AttackSpeed;           // 공격 속도
    public float MoveRange;             // 이동 범위
    public float AttackRange;           // 공격 범위

    public MonsterStatus(MonsterData monsterData)
    {
        Name = monsterData.Name;
        Description = monsterData.Description;
        Atk = monsterData.Atk;          
        Def = monsterData.Def;
        HP = monsterData.HP;
        Speed = monsterData.Speed;
        AttackSpeed = monsterData.AttackSpeed;
        MoveRange = monsterData.MoveRange;
        AttackRange = monsterData.AttackRange;
    }
}

// * MonsterController 스크립트
//- 이동, 공격, 충돌, 사망
public abstract class MonsterController : MonoBehaviour
{
    [SerializeField] protected MonsterData _monsterData;
    [SerializeField] protected GameObject _target;

    protected MonsterStatus _runtimeData;
    protected Animator _animator;
    protected AttackRangeController _attackRangeController;
    protected Rigidbody _rigidbody;

    //bool _isAttacking = false;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        MoveToTarget(_target.transform.position);
    }

    private void Start()
    {
        //Spawned();
    }

    // * 초기화 메서드
    //- 컴포넌트 연결 및 공격 범위 오브젝트 생성
    //- 공격 이벤트에 공격 메서드 구독
    public virtual void Initialize()
    {
        _runtimeData = new MonsterStatus(_monsterData);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _target = GameObject.Find(Define.PlayerTag);

        GameObject attackRange = new GameObject("AttackRange");
        attackRange.transform.parent = this.gameObject.transform;
        _attackRangeController = attackRange.GetOrAddComponent<AttackRangeController>();
        _attackRangeController.Initialize(_runtimeData.AttackRange);
        _attackRangeController.OnAttack += Attack;
        _attackRangeController.OffAttack += EndAttack;

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerTag), LayerMask.NameToLayer(Define.PlayerSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterTag), LayerMask.NameToLayer(Define.MonsterSkillLayer));
    }

    // 타겟 이동 메서드
    public virtual void MoveToTarget(Vector3 targetPos)
    {
        if(!_animator.GetBool(Define.IsAttacking))
        {
            Debug.Log("걷는중");
            
            Vector3 targetDir = (targetPos - transform.position).normalized;
            transform.position += targetDir * _runtimeData.Speed * Time.deltaTime;
            //transform.Translate(targetPos*Time.deltaTime);
            Debug.Log($"transform local position : {transform.localPosition}, targetPos : {targetPos}, targetDir.y : {targetDir}");
            Quaternion toRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10 * Time.deltaTime);

            _animator.SetFloat(Define.WalkSpeed, _runtimeData.Speed / 2);
            _animator.SetTrigger(Define.Walk);
        }
        else
        {
            _animator.SetFloat(Define.WalkSpeed, 0);
        }
    }

    // * 공격 메서드
    //- 추후 스킬 사용 몬스터 대비 가상함수로 구현
    public virtual void Attack()
    {
        if(!_animator.GetBool(Define.IsAttacking))
        {
            _animator.SetBool(Define.IsAttacking, true);
            _animator.SetTrigger(Define.Attack);
            Debug.Log("공격 중");
        }
    }

    // * 공격 종료 메서드
    //- 공격 플래그 false 및 종료 트리거 발생
    public virtual void EndAttack()
    {
        _animator.SetBool(Define.IsAttacking, false);
        _animator.SetTrigger(Define.EndAttack);
        Debug.Log("공격 종료");
    }

    // * 방어력 미적용 데미지 계산 메서드
    public void GetRealDamaged(float damage)
    {
        _runtimeData.HP -= damage;
        if (_monsterData.HP <= 0)
            Die();
    }

    // * 방어력 적용 데미지 계산 메서드
    public void GetDamaged(float damage)
    {
        float finalDamage = damage - _runtimeData.Def > 0 ? damage - _runtimeData.Def : 0;
        _runtimeData.HP -= damage;
        if (_runtimeData.HP <= 0)
            Die();
    }

    // * 사망 메서드
    //- 오브젝트 풀링 대비 비활성화 처리
    public virtual void Die()
    {
        Instantiate(_monsterData.DeadEffect);
        gameObject.SetActive(false);
    }

    // * 생성 메서드
    //- start주기함수에서 실행
    //public virtual void Spawned()
    //{
    //    Instantiate(_monsterData.SpawnEffect);
    //}

    // * 플레이어 충돌 및 공격 중일 시 데미지 계산
    //- stay로 처리했기에 피격 쿨타임을 적용 시켜 밸런스 조정 필요
    private void OnCollisionStay(Collision collision)
    {
        if(_animator.GetBool(Define.IsAttacking))
        {
            if(collision.gameObject.CompareTag(Define.PlayerTag))
            {
                // 공격 처리
                // collision.gameObject.GetComponent<PlayerController>().피격메서드;
                // 피격 메서드 내에서 피격 쿨타임 활용해보아도 될듯합니다.
            }
        }
    }
}
