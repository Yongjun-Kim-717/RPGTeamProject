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

    private void Awake()
    {
        Initialize();
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
        yield return new WaitForSeconds(2.0f);
        
        gameObject.SetActive(false);
        OnPotalClose?.Invoke();
    }
}
