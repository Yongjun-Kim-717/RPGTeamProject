using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    Rigidbody _rigidbody;
    [SerializeField] float _speed = 5f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Move();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        if (transform.position.z >= 108.2)
        {
            Vector3 pos = transform.position;
            pos.z = 5;
            transform.position = pos;
        }
        // ���� ���� �ƴϰ� wasd �Է� �� �̵�
        if (!_animator.GetBool(Define.IsAttacking) && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(h, 0, v);
            transform.Translate(movement.normalized * _speed * Time.deltaTime, Space.World);
            Vector3 pos=transform.position;
            pos.x = Mathf.Clamp(pos.x, -23, 23);
            pos.z = Mathf.Clamp(pos.z, 4, pos.z);                
            transform.position = pos;
            //_rigidbody.MovePosition(_rigidbody.position + movement.normalized * _speed * Time.deltaTime);

            _animator.SetFloat(Define.Speed, movement.magnitude);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), _speed * Time.deltaTime);
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _animator.SetFloat(Define.Speed, 0);
        }
        Debug.Log(_rigidbody.linearVelocity.y);
    }

    bool IsPossbleMove(Vector3 dir)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, dir, out hit, 1f);
        return true;
    }

    public void Attack()
    {
        if (!_animator.GetBool(Define.IsAttacking) && Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger(Define.Attack);
            _animator.SetBool(Define.IsAttacking, true);
        }
    }

    /// 
    /// ť + ��ųʸ� �̿��� ��ų Ȱ��
    /// 
    #region Skill Queue
    Queue<GameObject> _skillQueue = new Queue<GameObject>();
    Dictionary<SkillTest, GameObject> _skillDictionary = new Dictionary<SkillTest, GameObject>();

    [SerializeField] SkillTest[] skillList;
    public static Action<GameObject> OnEnqueueSkill;
    public static Action<SkillTest> OnSkillCoolTime;

    void InitializeSkill()
    {
        OnEnqueueSkill += EnqueueSkill;
        skillList = Resources.LoadAll<SkillTest>("");
        for (int i = 0; i < skillList.Length; i++)
        {
            _skillQueue.Enqueue(Instantiate(skillList[i]).gameObject);
        }
    }

    // ��Ÿ�� �ʱ�ȭ��, ��� ������ ��ų�� ť�� �ֱ�
    void EnqueueSkill(GameObject skill)
    {
        _skillQueue.Enqueue(skill);
    }

    // ť�� ��ų�� �ְ� ���� ������ ������ �߻� - Update �Լ��� �߰�
    void SkillAttack()
    {
        if (_skillQueue.Count > 0 && !_animator.GetBool(Define.IsAttacking))
        {
            StartCoroutine(_skillQueue.Dequeue().GetComponent<SkillTest>().TurnOnSkill(transform.position));
        }
    }

    // ��Ÿ�� ��� ���� �¿� ��ų �߰�
    public void AddDictionary(GameObject skill)
    {
        _skillDictionary.Add(skill.GetComponent<SkillTest>(), skill);
    }

    // ��Ÿ�� ���� ��ų �����ϰ� ť�� �߰�
    public void RemoveDictionary(GameObject skill)
    {
        _skillDictionary.Remove(skill.GetComponent<SkillTest>());
        _skillQueue.Enqueue(skill);
    }
    #endregion
}
