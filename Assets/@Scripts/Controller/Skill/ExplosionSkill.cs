using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class ExplosionSkill : NonTargetSkill
{
    WaitForSeconds _deactiveTime = new WaitForSeconds(1.5f);
    private GameObject _hitEffect;
    public override void Initialize()
    {
        base.Initialize();

        _hitEffect.SetActive(false);
    }

    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        StartCoroutine(DeactiveExplosion());
    }

    IEnumerator DeactiveExplosion()
    {
        yield return _deactiveTime;
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            //Vector3 bulletColliderCenter = this.GetComponent<Collider>().bounds.center;
            //Vector3 playerColliderCenter = other.bounds.center;
            //Vector3 hitEffectPos = (bulletColliderCenter + playerColliderCenter) / 2;

            _hitEffect.transform.position = other.bounds.center;
            _hitEffect.SetActive(true);
        }
    }
}
