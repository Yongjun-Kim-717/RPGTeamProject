using UnityEngine;

public class NonTargetSkill : ActiveSkill
{
    // NonTarget�̹Ƿ� target ���� �ʿ� ����
    public override void ActivateSkill(Transform target)
    {
        base.ActivateSkill(null);
        //gameObject.SetActive(true);
        //gameObject.GetComponent<ParticleSystem>()?.Play();
        //StartCoroutine(DeActivateSkill());
    }
}
