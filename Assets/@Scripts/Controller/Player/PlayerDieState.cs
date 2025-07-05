using UnityEngine;

public class PlayerDieState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsDead, true);
        //PlayerManager.Instance.IsDead = true;
    }
}
