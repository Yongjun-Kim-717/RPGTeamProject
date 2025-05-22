using UnityEngine;

public class SkillColliderController : MonoBehaviour
{
    float _speed;
    float _damage;
    float _distance;
    float _castingTime;

    float _currentTime = 0f;
    bool _isCasting = false;
    Vector3 _direction;
    GameObject _effect;

    public void SetColliderInfo(float speed, float damage, float distance, float casting, GameObject effect)
    {
        _speed = speed;
        _damage = damage;
        _distance = distance;
        _castingTime = casting;
        _effect = effect;
    }

    public void SetColliderDirection(Vector3 dir)
    {
        _direction = dir;
    }

    // activate�Ǹ�(Ȱ��ȭ�Ǹ�), ĳ���� ���� ����
    private void OnEnable()
    {
        if (_castingTime > 0)
        {
            _isCasting = true;
        }
    }

    void Update()
    {
        // ĳ���� ���� ������ ���� �� distance���� �̵�
        if (!_isCasting)
        {
            if (Vector3.Distance(transform.position, transform.parent.position) < _distance)
            {
                transform.Translate(_direction.normalized * _speed * Time.deltaTime);
                //InfiniteLoopDetector.Run();
            }
        }
        // ĳ���� ���� ���� ��
        else
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _castingTime)
            {
                _isCasting = false;
                _currentTime = 0f;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.MonsterTag))
        {
            other.GetComponent<MonsterController>().GetDamaged(_damage);
            // raycast�� �̿��� ��ų�� �浹 ���� ���
            RaycastHit hit;
            Vector3 direction = (other.transform.position - transform.position).normalized;
            Physics.Raycast(transform.position, direction, out hit);
            // �浹 �������� �ݴ� �������� hit effect �߻�
            GameObject effect = Instantiate(_effect, hit.point, Quaternion.Inverse(Quaternion.Euler(direction)));
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.5f);
        }
    }
}
