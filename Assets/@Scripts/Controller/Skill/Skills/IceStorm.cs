using UnityEngine;

public class IceStorm : NonTargetSkill, SpellMotion
{
    public void SpellAttack()
    {
        _animator.SetTrigger(Define.Spell);
        //_animator.SetBool(Define.IsAttacking, true);
    }

    public override void ActivateSkill(Transform target)
    {
        // ���� ��� ���� ��� ����� ����
        if (!_animator.GetBool(Define.IsAttacking))
        {
            SpellAttack();
        }
        base.ActivateSkill(target);
    }
}
