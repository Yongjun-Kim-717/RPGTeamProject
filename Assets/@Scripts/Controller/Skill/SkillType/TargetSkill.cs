using UnityEngine;

//Ÿ���� ��ų�� ������ ��ũ��Ʈ
public class TargetSkill : ActiveSkill
{
    protected override void ActivateSkill(Transform target)
    {
        base.ActivateSkill(target);
        //// Ÿ���� ��ų�� ���, ���� ���� �� �ݶ��̴� ��ġ ����
        //if (target != null)
        //{
        //    //Ÿ�� �������� ��ų ���� ����
        //    Vector3 dir = (target.position - _player.transform.position).normalized;
        //    _player.Rotate(dir);

        //    //�÷��̾� ��ġ�� ��ų Ȱ��ȭ
        //    transform.localPosition = Vector3.zero;
        //    _coll.transform.localPosition = Vector3.zero;
        //    _coll.SetColliderDirection(Vector3.forward);
        //}
        //gameObject.SetActive(true);
        //// particle system�� ���
        //gameObject.GetComponent<ParticleSystem>()?.Play();

        //StartCoroutine(DeActivateSkill()); //��ų ���� �� ��ų ��Ȱ��ȭ
    }
}
