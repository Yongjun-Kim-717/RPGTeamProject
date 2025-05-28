using UnityEngine;

public class MonsterSkillColliderController : MonoBehaviour
{
    float _damage;
    GameObject _connectedSkill;
    GameObject _effect;
    public void SetColliderInfo(float damage, GameObject connectedSkill, 
        GameObject effect)
    {
        _damage = damage;
        _connectedSkill = connectedSkill;
        _effect = effect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            GameObject effect = Instantiate(_effect);
            effect.transform.position = transform.position;
            if(_connectedSkill != null)
                transform.parent.gameObject.SetActive(false);
        }

        if (other.CompareTag(Define.GroundTag))
        {
            if (_connectedSkill != null)
            {
                GameObject connectedSkill = Instantiate(_connectedSkill);
                connectedSkill.transform.position = transform.position;
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
