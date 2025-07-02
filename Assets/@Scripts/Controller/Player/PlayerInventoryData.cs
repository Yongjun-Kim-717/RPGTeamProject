using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "Scriptable Objects/PlayerInventoryData")]
public class PlayerInventoryData : ScriptableObject
{
    public int level;
    public int silverCoin;
    public float exp;
    public int enhancementStone;

    public List<ItemData> Items;

    public event Action<Define.GoodsType> OnValueChanged;

    public bool ModifyGoods(Define.GoodsType type, float amount)
    {
        switch (type)
        {
            case Define.GoodsType.SilverCoin:
                if (silverCoin + amount <= 0) return false;
                silverCoin += (int)amount;
                break;
            case Define.GoodsType.Exp:
                exp += amount;
                if (exp >= 100)
                {
                    level++;
                    exp -= 100;
                }
                break;
            case Define.GoodsType.EnhancementStone:
                if (enhancementStone + amount <= 0) return false;
                enhancementStone += (int)amount;
                break;
            default:
                return false;
        }
        OnValueChanged?.Invoke(type);
        return true;
    }
}

public class Inventory
{
    private Dictionary<Define.ItemType, List<ItemData>> _items;

    public Dictionary<Define.ItemType, List<ItemData>> Items
    {
        get { return _items; }
    }

    public Inventory(PlayerInventoryData so)
    {
        _items = new();
        InitializeFromSO(so);
    }

    public void InitializeFromSO(PlayerInventoryData so)
    {
        foreach (var item in so.Items)
        {
            if (!_items.ContainsKey(item.Type))
                _items[item.Type] = new List<ItemData>();
            _items[item.Type].Add(item);
        }
    }

    public void AddItem(ItemData item)
    {
        if (!_items.ContainsKey(item.Type))
            _items[item.Type] = new List<ItemData>();
        _items[item.Type].Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        if (_items.TryGetValue(item.Type, out var list))
            list.Remove(item);
    }
}
