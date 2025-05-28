using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    float _damage;
    GameObject _effect;

    public void SetColliderInfo(float damage, GameObject effect)
    {
        _damage = damage;
        _effect = effect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.MonsterTag))
        {
            other.GetComponent<MonsterController>().GetDamaged(_damage);
            // raycast를 이용해 스킬의 충돌 지점 계산
            RaycastHit hit;
            Vector3 direction = (other.transform.position - transform.position).normalized;
            Physics.Raycast(transform.position, direction, out hit);
            // 충돌 지점에서 반대 방향으로 hit effect 발생
            GameObject effect1 = Instantiate(_effect, hit.point, Quaternion.LookRotation(hit.normal));
            effect1.name = "effect1";
            effect1.GetComponent<ParticleSystem>().Play();
            Destroy(effect1, 0.5f);
        }
    }
}
