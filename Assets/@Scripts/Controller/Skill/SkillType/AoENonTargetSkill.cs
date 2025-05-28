using System.Collections;
using UnityEngine;

public class AoENonTargetSkill : NonTargetSkill
{
    MonsterSkillColliderController _coll;
  
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<MonsterSkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _skillData.connectedSkillPrefab, _skillData.hitEffectPrefab);
    }
}
