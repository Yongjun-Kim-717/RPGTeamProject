using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

// * GameManager 스크립트
//- 
public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        Initialize();
        EventSubscribeAll();
        DeactivateObjectsAll();
    }

    // * 초기화 메서드
    protected override void Initialize()
    {
        base.Initialize();
        //스위치케이스
        ObjectManager.Instance.CreateManager();
        ObjectManager.Instance.ResourceLoad();

        DungeonManager.Instance.CreateManager();

        PopupUIManager.Instance.CreateManager();
    }

    // * 이벤트 구독 메서드
    private void EventSubscribeAll()
    {
        DungeonManager.Instance.Subscribe();
        PopupUIManager.Instance.Subscribe();
    }

    // * 오브젝트 비활성화 메서드
    private void DeactivateObjectsAll()
    {
        DungeonManager.Instance.Deactivate();
        PopupUIManager.Instance.Deactivate();
    }
}
