using System;
using System.Collections;
using UnityEngine;
using extension;

// * 던전 입구 Controller 스크립트
//- 던전 입구 포탈 관리 기능
public class DungeonPortalController : MonoBehaviour
{
    public Action OnPotalEnter;
    public Action OnPotalClose;

    private BoxCollider _boxCollider;

    public bool IsEnterPortal;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(ActiveTrueSelf());
    }

    void Initialize()
    {
        _boxCollider = gameObject.GetOrAddComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = new Vector3(1.5f, 2, 0.5f);
    }

    // * 트리거 엔터 시
    //- 던전 시스템 활성화
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPotalEnter?.Invoke();
            StartCoroutine(ActiveFalseSelf());
        }
    }

    IEnumerator ActiveFalseSelf()
    {
        yield return new WaitForSeconds(1.0f);
        
        gameObject.SetActive(false);
        OnPotalClose?.Invoke();
    }

    IEnumerator ActiveTrueSelf()
    {
        Vector3 targetScale = new Vector3(4.0f, 4.0f, 4.0f);
        while(true)
        {
            yield return null;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 0.4f);
            if (Vector3.Distance(transform.localScale, targetScale) <= 1f)
                break;
        }
    }
}
