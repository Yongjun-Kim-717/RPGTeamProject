using UnityEngine;

public class Define : MonoBehaviour
{
    #region Animator
    public readonly static int Idle = Animator.StringToHash("Idle");
    public readonly static int Attack = Animator.StringToHash("Attack");
    public readonly static int Spell = Animator.StringToHash("Spell");
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int Walk = Animator.StringToHash("Walk");

    public readonly static int CloseAttack = Animator.StringToHash("CloseAttack");
    public readonly static int LongAttack = Animator.StringToHash("LongAttack");

    public const string EndAttack = "EndAttack";
    public const string WalkSpeed = "WalkSpeed";
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string MonsterTag = "Monster";
    public const string GroundTag = "Ground";
    #endregion

    #region Layer
    public const string PlayerSkillLayer = "PlayerSkill";
    public const string MonsterSkillLayer = "MonsterSkill";
    #endregion

    #region Path
    public const string DemonPath = "Monsters/Demon";
    #endregion

    #region SpawnSpots
    public Vector3 SpawnSpot1 = new Vector3();
    #endregion

    #region enum
    public enum SkillType
    {
        RigidBodyTarget,
        TransformTarget,
        AoENonTarget,
        DirectionNonTarget,
        Buff,
    }

    public enum MotionType
    {
        None,
        Sword,
        Spell,
    }

    public enum HandlerType
    {
        Player,
        Monster,
        Skill
    }
    #endregion
}
