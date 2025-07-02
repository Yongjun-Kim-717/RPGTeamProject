using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "Scriptable Objects/PlayerInventoryData")]
public class PlayerInventoryData : ScriptableObject
{
    public int SilverCoin;
    public int Gem;

    public List<ItemData> Items;
}

// 변경사항 구조체
// 아이템 변경 사항 내용
// 1. 아이템 생성 및 삭제
// 2. 아이템 개수 변환
// 3. 
public struct PendingChange
{

}

// 대기중인 변경사항 관리 클래스
public class PendingChangeTaskManager
{
    private Dictionary<Define.PendingTaskType, Queue<PendingChange>> _pendingChangeList;

    PendingChangeTaskManager()
    {
        _pendingChangeList = new Dictionary<Define.PendingTaskType, Queue<PendingChange>>();

        foreach(Define.PendingTaskType taskType in Enum.GetValues(typeof(Define.PendingTaskType)))
        {
            _pendingChangeList.Add(taskType, new Queue<PendingChange>());
        }
    }

}

// 실제 인벤토리 클래스
public class Inventory
{
    private Dictionary<Define.GoodsType, int> _goods;
    private Dictionary<Define.ItemType, List<ItemData>> _items;

    public Dictionary<Define.ItemType, List<ItemData>> Items
    {
        get { return _items; }
        set { _items = value; }
    }

    public event Action<Dictionary<Define.ItemType, List<ItemData>>> OnInventorySet;
    public event Action<Define.GoodsType> OnValueChanged;
    public event Action<Define.ItemType> OnItemAdd;
    public event Action<Define.ItemType> OnItemRemove;

    // 생성자
    public Inventory(PlayerInventoryData so)
    {
        _goods = new();
        _items = new();
        InitializeFromSO(so);
    }

    public void InitializeFromSO(PlayerInventoryData so)
    {
        _goods[Define.GoodsType.SilverCoin] = so.SilverCoin;
        _goods[Define.GoodsType.Gem] = so.Gem;

        foreach (var item in so.Items)
        {
            if (!_items.ContainsKey(item.Type))
                _items[item.Type] = new List<ItemData>();
            _items[item.Type].Add(item);
        }

        OnInventorySet?.Invoke(_items);
    }

    public void AddItem(ItemData item)
    {
        if (!_items.ContainsKey(item.Type))
            _items[item.Type] = new List<ItemData>();
        _items[item.Type].Add(item);
        OnItemAdd?.Invoke(item.Type);
    }

    public void RemoveItem(ItemData item)
    {
        if (_items.TryGetValue(item.Type, out var list))
            list.Remove(item);
        OnItemRemove?.Invoke(item.Type);
    }
}
