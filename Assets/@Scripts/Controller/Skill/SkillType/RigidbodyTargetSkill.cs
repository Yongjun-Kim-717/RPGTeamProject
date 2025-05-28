using UnityEngine;

public class RigidbodyTargetSkill : TargetSkill
{

    MonsterSkillColliderController _coll;
    Rigidbody _rigidbody;

    public override void Initialize()
    {
        base.Initialize();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _coll = GetComponentInChildren<MonsterSkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.gameObject.transform.localPosition = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = dir * _skillData.force;
    }
}
