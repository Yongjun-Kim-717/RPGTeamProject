using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "Scriptable Objects/PlayerInventoryData")]
public class PlayerInventoryData : ScriptableObject
{
    public int SilverCoin;
    public int Gem;

    public List<ItemData> Items;
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

    public Dictionary<Define.GoodsType, int> Goods
    {
        get { return _goods; }
        set { _goods = value; }
    }

    public event Action<Dictionary<Define.ItemType, List<ItemData>>> OnInventorySet;
    public event Action<Define.GoodsType> OnValueChanged;
    public event Action<Define.ItemType, Define.PendingTaskType, int> OnItemAdd;
    public event Action<Define.ItemType, Define.PendingTaskType, int> OnItemRemove;


    // 생성자
    public Inventory(PlayerInventoryData so)
    {
        _goods = new();
        _items = new();
        InitializeFromSO(so);
    }

    // Scriptable Object로부터 초기화
    private void InitializeFromSO(PlayerInventoryData so)
    {
        _goods[Define.GoodsType.SilverCoin] = so.SilverCoin;
        _goods[Define.GoodsType.Gem] = so.Gem;

        // 아이템이 존재하지 않더라도 모든 타입의 인벤토리는 미리 생성을 해둬야 함
        foreach (Define.ItemType itemType in Enum.GetValues(typeof(Define.ItemType)))
        {
            _items[itemType] = new List<ItemData>();
        }

        // DB의 아이템 리스트를 모두 순회하며 클라이언트 타입별 인벤토리에 데이터 저장
        foreach (var item in so.Items)
        {
            if (_items.ContainsKey(item.Type))
                _items[item.Type].Add(item);
        }

        OnInventorySet?.Invoke(_items);
    }

    // 변경사항 Scriptable Object에 동기화
    public void ApplyChangesToSO(PlayerInventoryData so)
    {
        so.SilverCoin = _goods[Define.GoodsType.SilverCoin];
        so.Gem = _goods[Define.GoodsType.Gem];

        so.Items.Clear();
        foreach (Define.ItemType itemType in Enum.GetValues(typeof(Define.ItemType)))
        {
            for(int i = 0; i < _items[itemType].Count; i++)
            {
                so.Items.Add(_items[itemType][i]);
            }
        }
    }

    // 아이템 추가 메서드
    public void AddItem(ItemData item,int count)
    {
        if (!_items.ContainsKey(item.Type))
            _items[item.Type] = new List<ItemData>();
        if(item.Type == Define.ItemType.Equipment)
        {
            for(int i = 0; i < count; i++)
                _items[item.Type].Add(item);
        }
        else
        {
            List<ItemData> list = _items[item.Type];
            ItemData existItem = list.FirstOrDefault(i => i.Id == item.Id);

            if (existItem == null)
            {
                item.Count++;
                _items[item.Type].Add(item);
            }
            else
            {
                existItem.Count += count;
            }
        }
    }

    // 아이템 추가 메서드 (매개변수 딕셔너리 : Key -> SkillData, ItemData)
    public void AddItem(Dictionary<Data,int> items)
    {
        foreach (var item in items)
        {
            AddItem(item.Key as ItemData, item.Value);
        }
    }

    // 개선필요
    // 개수작업 안되어있음.
    public void RemoveItem(ItemData item)
    {
        if (_items.TryGetValue(item.Type, out var list))
        {
            ItemData existItem = list.FirstOrDefault(i => i.Id == item.Id);
            if (existItem == null)
                return;
            existItem.Count--;
            if (existItem.Count <= 0)
                _items[item.Type].Remove(existItem);
        }
    }

    // 재화 추가 메서드
    public void AddGoods(Define.GoodsType type, int amount)
    {
        _goods[type] += amount;
        ApplyChangesToSO(PlayerManager.Instance.Player.PlayerInventoryData);
        OnValueChanged?.Invoke(type);
    }

    // 재화 사용 메서드
    public void UseGoods(Define.GoodsType type, int amount)
    {
        _goods[type] -= amount;
        ApplyChangesToSO(PlayerManager.Instance.Player.PlayerInventoryData);
        OnValueChanged?.Invoke(type);
    }
}
