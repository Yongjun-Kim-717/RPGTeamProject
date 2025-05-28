using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public float coolTime;
    public float durationTime;
    public float castingTime;
    public float targetDistance;
    public float damage;
    public float speed;
    public float force;
    public GameObject connectedSkillPrefab;
    public GameObject hitEffectPrefab;
    public Define.SkillType skillType;
    public Define.MotionType motionType;
    public Define.HandlerType handlerType;
}

