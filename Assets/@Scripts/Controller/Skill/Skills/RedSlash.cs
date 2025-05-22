using UnityEngine;

public class RedSlash : TargetSkill, SwordMotion
{
    Vector3 _euler = new Vector3(0, 0, 90f);

    public void SwordAttack()
    {
        _animator.SetTrigger(Define.Attack);
        //_animator.SetBool(Define.IsAttacking, true);
    }

    public override void ActivateSkill(Transform target)
    {
        // ���� ��� ���� ��� ����� ����
        if (!_animator.GetBool(Define.IsAttacking))
        {
            SwordAttack();
        }
        // �˱��� ���, 90�� ȸ���ؼ� ������ ���·� �߻�
        transform.localRotation = Quaternion.Euler(_euler);
        base.ActivateSkill(target);
    }
}
