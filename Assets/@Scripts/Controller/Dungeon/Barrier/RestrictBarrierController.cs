using System.Collections;
using UnityEngine;

public class RestrictBarrierController : MonoBehaviour
{
    private BoxCollider _boxCollider;
    public Material _material;
    public ParticleSystem _particleSystem;
    public float DurationTime = 1.5f;

    public GameObject WavePrefab;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _particleSystem = WavePrefab.GetComponent<ParticleSystem>();
        //_material = GetComponent<Material>();
    }

    // * 던전 벽 충돌 시 Fade 처리 및 충돌 이펙트 생성
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(FadeIn());
            StartCoroutine(FadeIn());
            Instantiate(WavePrefab, new Vector3(collision.gameObject.transform.position.x, 2, transform.position.z), Quaternion.AngleAxis(90f, Vector3.right));
            //_particleSystem.Play();
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    float coolTime = 10f;
    //    if(collision.gameObject.CompareTag("Player"))
    //    {
    //        if(coolTime >= 10f)
    //        {
    //            Instantiate(WavePrefab, new Vector3(collision.gameObject.transform.position.x, 2, transform.position.z), Quaternion.AngleAxis(90f, Vector3.right));
    //            coolTime = 0f;
    //        }
    //        coolTime += Time.deltaTime;
    //    }
    //}

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        Color color = _material.color;

        while(color.a < 0.3f)
        {
            color.a += Time.deltaTime / DurationTime;
            _material.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        Color color = _material.color;

        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime / DurationTime;
            _material.color = color;
            yield return null;
        }
    }
}
