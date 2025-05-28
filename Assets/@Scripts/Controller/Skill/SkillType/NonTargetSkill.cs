using UnityEngine;

public class NonTargetSkill : ActiveSkill
{
    // NonTarget이므로 target 받을 필욘 없
    public override void ActivateSkill(Transform target = null, Vector3 pos = default)
    {
        base.ActivateSkill(null, pos);
    }
}
