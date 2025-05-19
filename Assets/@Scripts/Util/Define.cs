using UnityEngine;

public class Define : MonoBehaviour
{
    #region Animator
    public readonly static int Attack = Animator.StringToHash("Attack");
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string MonsterTag = "Monster";
    #endregion
    #region Animation
    //public const string Attack = "Attack";
    public const string EndAttack = "EndAttack";
    public const string Walk = "Walk";
    public const string WalkSpeed = "WalkSpeed";
    #endregion

    #region Path
    public const string DemonPath = "Monsters/Demon";
    #endregion

    #region SpawnSpots
    public Vector3 SpawnSpot1 = new Vector3();
    #endregion
}
