using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/EquipmentItem")]
public class EquipmentItemData : ItemData
{
    public Define.EquipmentItemType EquipmentType;
    public ItemStatus ItemStatus;
}

[System.Serializable]
public class ItemStatus
{
    public float Atk;                       //공격력
    public float Def;                       //방어력
    public float HP;                        //체력
    public float HPRecoveryPerSec;          //체력회복
    public float CoolTimeDecrease;          //쿨타임 감소
    public float Speed;                     //이동속도
}
