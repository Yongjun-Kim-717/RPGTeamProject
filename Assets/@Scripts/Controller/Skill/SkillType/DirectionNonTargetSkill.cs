using UnityEngine;

public class DirectionNonTargetSkill : ActiveSkill, ICheckActivation
{
    SkillColliderController _coll;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
    }

    // 스킬 발동 순간, 그 앞의 범위 내에 있는 적들에게 대미지
    public override bool ActivateSkill(Vector3 pos)
    {
        // 거리 내 타겟이 있어야 발동하게 바꿈
        if (IsActivatePossible(pos))
        {
            base.ActivateSkill(pos);

            // 현재 플레이어가 바라보는 방향 == 스킬 발동 방향
            transform.rotation = _player.transform.rotation;

            return true;
        }
        return false;
    }

    public bool IsActivatePossible(Vector3 pos)
    {
        // 수동 모드일 땐 타겟 유무 상관없이 그냥 발사
        if (SkillData.IsPlayerSkill && !PlayerManager.Instance.IsAuto)
            return true;
        return _player.Target != null && Vector3.Distance(_player.Target.position, pos) <= _skillData.TargetDistance;
    }
}
