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

    [SerializeField] GameObject _helmetEquipment;
    [SerializeField] GameObject _armorEquipment;
    [SerializeField] GameObject _glovesEquipment;
    [SerializeField] GameObject _shoesEquipment;
    [SerializeField] GameObject _weaponEquipment;

    // 장착된 아이템 슬롯 리스트
    Dictionary<Define.EquipmentItemType, ItemSlot> _equipmentItemSlots = new Dictionary<Define.EquipmentItemType, ItemSlot>();
    // 아이템 슬롯 리스트
    Dictionary<Define.ItemType, List<ItemSlot>> _itemSlots = new Dictionary<Define.ItemType, List<ItemSlot>>();

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
        Initialize();
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
        SetViewPort(Define.ItemType.Equipment);
    }

    // * 초기화 메서드
    //- Awake에서 호출
    private void Initialize()
    {
        // 장비창 슬롯 초기화
        _equipmentItemSlots.Clear();

        // 장비창 슬롯 키 생성
        foreach (Define.EquipmentItemType equipmentItemType in Enum.GetValues(typeof(Define.EquipmentItemType)))
        {
            _equipmentItemSlots.Add(equipmentItemType, null);
        }

        // 아이템 슬롯 리스트 동적할당
        _itemSlots[Define.ItemType.Equipment] = new List<ItemSlot>();
        _itemSlots[Define.ItemType.Consumable] = new List<ItemSlot>();
        _itemSlots[Define.ItemType.Other] = new List<ItemSlot>();

        // 버튼 클릭 리스너 연결
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _equipmentItemButton.onClick.AddListener(OnEquipmentButtonClick);
        _consumeItemButton.onClick.AddListener(OnConsumeButtonClick);
        _otherItemButton.onClick.AddListener(OnOtherButtonClick);
    }

    // * 슬롯 생성 메서드
    //- 인벤토리 첫 오픈 시 아이템 슬롯 생성
    void CreateSlot(Define.ItemType itemType, List<ItemData> itemsList)
    {
        List<ItemData> itemList = itemsList;

        Transform viewPort = GetViewPortTransform(itemType);

        foreach (var itemData in itemList)
        {
            string slotName = itemData.Value switch
            {
                Define.ItemValue.Common => "ItemSlot - Common",
                Define.ItemValue.Uncommon => "ItemSlot - Uncommon",
                Define.ItemValue.Rare => "ItemSlot - Rare",
                Define.ItemValue.Epic => "ItemSlot - Epic",
                Define.ItemValue.Legendary => "ItemSlot - Legendary",
                _ => "ItemSlot - Normal"
            };

            GameObject itemSlot = PoolManager.Instance.GetObjectFromPool<ItemSlot>(Vector3.zero, slotName, viewPort.GetChild(0).GetChild(0));
            itemSlot.GetComponent<ItemSlot>().SetData(itemData, true);
            if(itemType != Define.ItemType.Equipment)
                itemSlot.GetComponent<ItemSlot>().SetItemCount(itemData.Count);

            _itemSlots[itemType].Add(itemSlot.GetComponent<ItemSlot>());
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

    //// * 인벤토리 동기화 메서드
    ////- 탭의 첫 오픈이 아닐 경우 호출
    ////- 변경 사항 대기 큐에 존재하는 작업 내용을 수행
    //void UpdateInventory(Define.ItemType itemType)
    //{
    //while(PlayerManager.Instance.InventoryChangeQueue.IsExistTask(itemType))
    //{
    //    PendingChange pc = PlayerManager.Instance.InventoryChangeQueue.PopTask(itemType);
    //    switch (pc.TaskType)
    //    {
    //        case Define.PendingTaskType.ItemAddTask:
    //            CreateSlot(itemType, )
    //            break;
    //        case Define.PendingTaskType.ItemRemoveTask:
    //            break;
    //        case Define.PendingTaskType.ItemUpdateTask:
    //            break;
    //        default:
    //            break;
    //    }
    //}
    //}

    // * 각 버튼 클릭 리스너 이벤트에서 메서드 실행
    //- 아이템 타입 별 뷰포트 활성화 메서드 수행
    public void OnEquipmentButtonClick() => OnTabButtonClick(Define.ItemType.Equipment);
    public void OnConsumeButtonClick() => OnTabButtonClick(Define.ItemType.Consumable);
    public void OnOtherButtonClick() => OnTabButtonClick(Define.ItemType.Other);
    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
        AudioManager.Instance.PlayClickSound();
    }

    // * 아이템 타입 별 버튼 클릭 메서드
    //- 아이템 타입 별 뷰포트 내용 동기화
    void OnTabButtonClick(Define.ItemType itemType)
    {
        SetViewPort(itemType);
        AudioManager.Instance.PlayClickSound();
    }

    void SetViewPort(Define.ItemType itemType)
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

        ClearSlots(itemType);
        CreateSlot(itemType, PlayerManager.Instance.Player.Inventory.Items[itemType]);
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

    // * 모든 슬롯 정리
    void ClearSlots(Define.ItemType itemType)
    {
        Transform content = GetViewPortTransform(itemType).GetChild(0).GetChild(0);

        for (int i = 0; i < content.childCount; i++)
        {
            GameObject slotObj = content.GetChild(i).gameObject;
            slotObj.SetActive(false);
        }

        _itemSlots[itemType].Clear();
    }

    // 아이템 슬롯 장착 메서드
    public void EquipItem(ItemSlot _itemSlot)
    {
        Define.EquipmentItemType itemType = ((EquipmentItemData)_itemSlot.ItemData).EquipmentType;

        if (_itemSlots[Define.ItemType.Equipment].Contains(_itemSlot))
        {
            UnEquipItem(_equipmentItemSlots[itemType]);
            _equipmentItemSlots[itemType] = _itemSlot;
            _itemSlot.gameObject.transform.SetParent(GetEquipmentTransform(itemType));
            _itemSlot.gameObject.transform.localPosition = Vector3.zero;
            PlayerManager.Instance.Player.ApplyItemStatus(((EquipmentItemData)(_itemSlot.ItemData)).ItemStatus);
        }
    }

    // 아이템 사용 메서드
    public void UseItem(ItemSlot _itemSlot)
    {
        PlayerManager.Instance.Player.Inventory.RemoveItem(_itemSlot.ItemData);
        int count = _itemSlot.ItemData.Count; //해당 아이템 카운트 감소
        //아이템 개수가 0보다 작으면 
        //해당 아이템 슬롯 삭제 및 Inventory Data에서 해당 아이템 없애기
        if (count <= 0)
        {
            _itemSlots[_itemSlot.ItemData.Type].Remove(_itemSlot);
            _itemSlot.gameObject.SetActive(false);
            return;
        }
        _itemSlot.SetItemCount(count);
    }

    // 아이템 슬롯 장착 해제 메서드
    public void UnEquipItem(ItemSlot _itemSlot)
    {
        if (_itemSlot == null)
            return;

        Define.EquipmentItemType itemType = ((EquipmentItemData)_itemSlot.ItemData).EquipmentType;
        if (_equipmentItemSlots[((EquipmentItemData)_itemSlot.ItemData).EquipmentType] == _itemSlot)
        {
            _itemSlot.gameObject.transform.SetParent(_equipmentViewPort.transform.GetChild(0).GetChild(0));
            _itemSlot.gameObject.transform.localPosition = Vector3.zero;
            PlayerManager.Instance.Player.ReleaseItemStatus(((EquipmentItemData)_itemSlot.ItemData).ItemStatus);
            _equipmentItemSlots[itemType] = null;
        }
    }

    // 부위별 장착 위치 반환 메서드
    Transform GetEquipmentTransform(Define.EquipmentItemType itemType)
    {
        return itemType switch
        {
            Define.EquipmentItemType.Helmet => _helmetEquipment.transform,
            Define.EquipmentItemType.Weapon => _weaponEquipment.transform,
            Define.EquipmentItemType.Gloves => _glovesEquipment.transform,
            Define.EquipmentItemType.Shoes => _shoesEquipment.transform,
            Define.EquipmentItemType.Armor => _armorEquipment.transform,
            _ => null,
        };
    }
}
