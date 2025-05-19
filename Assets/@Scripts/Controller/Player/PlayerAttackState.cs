using UnityEngine;

public class PlayerAttackState : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (stateInfo.normalizedTime > 0.9f)
        {
            animator.SetBool("IsAttacking", false);
        }
    }
}
