using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 인벤토리 UI 스크립트
//- 버튼 이벤트 연결
//- PlayerInventoryData의 정보를 바탕으로 UI에 생성
[RequireComponent(typeof(EventTrigger))]
public class PopupUI_Inventory : MonoBehaviour
{
    // 슬롯 배치 뷰포트 및 버튼 직렬화 (인벤토리 UI 프리팹의 인스펙터를 통해 미리 연결)
    [SerializeField] GameObject _equipmentViewPort;
    [SerializeField] GameObject _consumeViewPort;
    [SerializeField] GameObject _otherViewPort;
    [SerializeField] Button _equipmentItemButton;
    [SerializeField] Button _consumeItemButton;
    [SerializeField] Button _otherItemButton;
    [SerializeField] Button _exitButton;

    // PopupUIManager에서 DeactivatePopup 연결
    public event Action OnExitButtonClicked;

    // 아이템 타입 별 뷰포트 첫 오픈 체크 변수
    bool _isFirstOpenEquipment = true;
    bool _isFirstOpenConsume = true;
    bool _isFirstOpenOther = true;

    // * Awake 주기함수
    //- 버튼 리스너 이벤트 연결
    private void Awake()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _equipmentItemButton.onClick.AddListener(OnEquipmentButtonClick);
        _consumeItemButton.onClick.AddListener(OnConsumeButtonClick);
        _otherItemButton.onClick.AddListener(OnOtherButtonClick);
    }

    // * 파괴 주기함수
    // 파괴 시 bool 타입 변수 초기화
    private void OnDestroy()
    {
        _isFirstOpenEquipment = true;
        _isFirstOpenConsume = true;
        _isFirstOpenOther = true;
    }

    // * 활성화 주기함수
    //- 활성화 시 기본 세팅인 장비창으로 탭 활성화
    private void OnEnable()
    {
        OnTabButtonClick(Define.ItemType.Equipment);
    }

    // * 슬롯 생성 메서드
    //- 인벤토리 첫 오픈 시 아이템 슬롯 생성
    void CreateSlot(Define.ItemType itemType)
    {
        List<ItemData> itemList = PlayerManager.Instance.Player.Inventory.Items[itemType];

        Transform viewPort = GetViewPortTransform(itemType);

        foreach (var itemData in itemList)
        {
            string slotName = itemData.Value switch
            {
                Define.ItemValue.Normal => "ItemSlot - Normal",
                Define.ItemValue.Rare => "ItemSlot - Rare",
                Define.ItemValue.Epic => "ItemSlot - Epic",
                Define.ItemValue.Unique => "ItemSlot - Unique",
                Define.ItemValue.Legendary => "ItemSlot - Legendary",
                _ => "ItemSlot - Normal"
            };

            PoolManager.Instance.GetObjectFromPool<ItemSlot>(Vector3.zero, slotName, viewPort);
        }
    }
    // * 아이템 타입에 따른 뷰포트 return 메서드
    Transform GetViewPortTransform(Define.ItemType itemType)
    {
        return itemType switch
        {
            Define.ItemType.Equipment => _equipmentViewPort.transform,
            Define.ItemType.Consumable => _consumeViewPort.transform,
            Define.ItemType.Other => _otherViewPort.transform,
            _ => null,
        };
    }

    // * 인벤토리 동기화 메서드
    //- 탭의 첫 오픈이 아닐 경우 호출
    //- 변경 사항 대기 큐에 존재하는 작업 내용을 수행
    void UpdateInventory(Define.ItemType itemType)
    {

    }

    // * 각 버튼 클릭 리스너 이벤트에서 메서드 실행
    //- 아이템 타입 별 뷰포트 활성화 메서드 수행
    public void OnEquipmentButtonClick() => OnTabButtonClick(Define.ItemType.Equipment);
    public void OnConsumeButtonClick() => OnTabButtonClick(Define.ItemType.Consumable);
    public void OnOtherButtonClick() => OnTabButtonClick(Define.ItemType.Other);
    void OnExitButtonClick() => OnExitButtonClicked?.Invoke();

    // * 아이템 타입 별 버튼 클릭 메서드
    //- 아이템 타입 별 뷰포트 내용 동기화
    void OnTabButtonClick(Define.ItemType itemType)
    {
        // 모든 탭 비활성화
        _equipmentViewPort?.SetActive(false);
        _consumeViewPort?.SetActive(false);
        _otherViewPort?.SetActive(false);

        // 해당 탭만 활성화
        GetViewPortTransform(itemType).gameObject.SetActive(true);

        // 첫 오픈 여부 판단
        bool isFirstOpen = itemType switch
        {
            Define.ItemType.Equipment => _isFirstOpenEquipment,
            Define.ItemType.Consumable => _isFirstOpenConsume,
            Define.ItemType.Other => _isFirstOpenOther,
            _ => false
        };

        if (isFirstOpen)
        {
            CreateSlot(itemType);
            // 첫 오픈 플래그 false 처리 필요
            SetFirstOpenFlag(itemType, false);
        }
        else
        {
            UpdateInventory(itemType);
        }
    }
    // * 아이템 타입 별 bool 변수 Set
    void SetFirstOpenFlag(Define.ItemType itemType, bool value)
    {
        switch (itemType)
        {
            case Define.ItemType.Equipment: _isFirstOpenEquipment = value; break;
            case Define.ItemType.Consumable: _isFirstOpenConsume = value; break;
            case Define.ItemType.Other: _isFirstOpenOther = value; break;
        }
    }
}
