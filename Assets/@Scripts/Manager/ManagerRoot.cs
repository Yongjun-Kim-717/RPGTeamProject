using System;
using System.Collections.Generic;
using UnityEngine;

// * GameManager 스크립트
//- 첫 씬에서 미리 배치
//- DontDestroyOnLoad로 모든 씬에 존재함.
public class ManagerRoot : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
        CreateManagers();
        EventSubscribeAll();
        DeactivateObjectsAll();
        Settings();
    }

    // * 초기화 메서드
    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    // * 매니저 생성 메서드
    private void CreateManagers()
    {
        ObjectManager.Instance.CreateManager();
        ObjectManager.Instance.ResourceLoad();

        PlayerManager.Instance.CreateManager();

        FieldManager.Instance.CreateManager();

        UIManager.Instance.CreateManager();

        PoolManager.Instance.CreateManager();

        PopupUIManager.Instance.CreateManager();


        DamageTextManager.Instance.CreateManager();

        SkillManager.Instance.CreateManager();

        StageManager.Instance.CreateManager();

        CutSceneManager.Instance.CreateManager();

        TimeManager.Instance.CreateManager();

        EnvironmentManager.Instance.CreateManager();

        CameraManager.Instance.CreateManager();
    }

    // * 이벤트 구독 메서드
    private void EventSubscribeAll()
    {
        FieldManager.Instance.Subscribe();
        PopupUIManager.Instance.Subscribe();
        DamageTextManager.Instance.Subscribe();
        StageManager.Instance.Subscribe();
        CutSceneManager.Instance.Subscribe();
        TimeManager.Instance.Subscribe();
        EnvironmentManager.Instance.Subscribe();
    }

    // * 오브젝트 비활성화 메서드
    private void DeactivateObjectsAll()
    {
        PopupUIManager.Instance.Deactivate();
        DamageTextManager.Instance.Deactivate();
        CutSceneManager.Instance.Deactivate();
    }
    
    private void Settings()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerTag), LayerMask.NameToLayer(Define.PlayerSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterTag), LayerMask.NameToLayer(Define.MonsterSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterAttackRangeLayer), LayerMask.NameToLayer(Define.MonsterSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterAttackRangeLayer), LayerMask.NameToLayer(Define.PlayerSkillLayer));
    }    
}
