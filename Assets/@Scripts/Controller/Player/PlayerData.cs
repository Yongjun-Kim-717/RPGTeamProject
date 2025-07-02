using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData" ,menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : Status
{
    public string Name;                     //이름
    public int Level;                       //레벨
    public float Exp;                       //경험치
    public float Damage;                    //기본대미지
    public float HP;                        //체력
    public float HPRecoveryPerSec;          //체력회복
    public float CoolTimeDecrease;          //쿨타임 감소
    public int UnlockedSkillSlotCount;      //현재 오픈된 스킬 슬롯 개수
    public float JourneyExp;                //모험 게이지
    public float Speed;
    public JourneyRankData JourneyRankData;
    public SkillData[] CurrentSkillData;
}
