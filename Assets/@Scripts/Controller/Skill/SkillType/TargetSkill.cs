using System;
using UnityEngine;

//타겟형 스킬에 부착될 스크립트
public class TargetSkill : ActiveSkill
{
    public event Action<Vector3> OnSkillSet;

    protected Vector3 _direction;

    // 스킬의 방향 설정 추가
    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        //타겟 방향으로 스킬 방향 설정
        _direction = (target.position - transform.position).normalized;
        //_handler.Rotate(dir);
        OnSkillSet?.Invoke(_direction);
    }
}
