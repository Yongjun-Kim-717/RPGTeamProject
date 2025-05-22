using UnityEngine;

public class ActiveSkill : Skill
{
    protected Animator _animator;
    protected SkillColliderController _coll;

    protected override void ActivateSkill(Transform target)
    {
        // Ÿ���� ��ų�� ���, ���� ���� �� �ݶ��̴� ��ġ ����
        if (target != null)
        {
            //Ÿ�� �������� ��ų ���� ����
            Vector3 dir = (target.position - _player.transform.position).normalized;
            _player.Rotate(dir);

            _coll.SetColliderDirection(Vector3.forward);
        }
        //�÷��̾� ��ġ�� ��ų Ȱ��ȭ
        transform.localPosition = Vector3.zero;
        _coll.transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        // particle system�� ���
        gameObject.GetComponent<ParticleSystem>()?.Play();

        StartCoroutine(DeActivateSkill()); //��ų ���� �� ��ų ��Ȱ��ȭ
    }

    public override void Initialize(SkillData data)
    {
        base.Initialize(data);
        _animator = _player.GetComponent<Animator>();
        SkillColliderController[] colls = GetComponentsInChildren<SkillColliderController>();
        foreach (var coll in colls)
            Debug.Log(coll.name);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.speed, _skillData.damage, _skillData.targetDistance, _skillData.castingTime, _skillData.hitEffectPrefab);
    }
}
