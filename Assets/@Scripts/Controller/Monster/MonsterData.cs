using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/Monsters")]
public class MonsterData : ScriptableObject
{
    public GameObject SpawnEffect;      // 스폰 이펙트
    public GameObject DeadEffect;       // 사망 이펙트
    public string Name;                 // 이름
    public string Description;          // 설명
    public float Atk;                   // 공격력
    public float Def;                   // 방어력
    public float HP;                    // 체력
    public float Speed;                 // 이동 속도
    public float AttackSpeed;           // 공격 속도
    public float MoveRange;             // 이동 범위
    public float AttackRange;           // 공격 범위
}
