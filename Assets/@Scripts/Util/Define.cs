using UnityEngine;

public class Define : MonoBehaviour
{
    #region Animator
    public readonly static int Attack = Animator.StringToHash("Attack");
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int Walk = Animator.StringToHash("Walk");
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string MonsterTag = "Monster";
    #endregion

    #region Animation
    public const string EndAttack = "EndAttack";
    public const string WalkSpeed = "WalkSpeed";
    #endregion

    #region Path
    public const string PlayerPath = "Player/Player";
    public const string MonsterPath = "Monsters/Normal";
    public const string NamedMonsterPath = "Monsters/Named";
    public const string MonsterSkillPath = "Skills/MonsterSkills/Skill";
    public const string MonsterSkillHitEffectPath = "Skills/MonsterSkills/Hit";
    public const string PlayerSkillPath = "Skills/PlayerSkills/Skill";
    public const string PlayerSkillHitEffectPath = "Skills/MonsterSkills/Hit";
    public const string ShieldEffectPath = "Dungeon/Object/ShieldEffect";
    public const string DungeonWallPath = "Dungeon/Object/DungeonWall";
    public const string DungeonPortalPath = "Dungeon/Object/DungeonPortal";

    public const string PopupUICanvasPath = "UI/PopupUI/Canvas - Popup";
    public const string PopupEnterDungeonPanelPath = "UI/PopupUI/Panel - Popup";
    #endregion

    #region SpawnSpots
    public Vector3 SpawnSpot1 = new Vector3();
    #endregion

    #region Spots
    public Vector3 FirstEnterSpot = new Vector3(0, 3, 0);
    public Vector3 DungeonEnterSpot = new Vector3(0, 30, 0);
    public Vector3 DungeonExitSpot = new Vector3(0, 100, 0);
    #endregion

}
