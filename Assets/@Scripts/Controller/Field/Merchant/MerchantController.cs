using UnityEngine;

public class MerchantController : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            _animator.SetTrigger(Define.Contact);
        }
    }
}
