using UnityEngine;
using System.Collections;

public class UIAnimatorCommonState : StateMachineBehaviour
{
	// public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	// {
	// 	Debug.LogError("ok on state");
	// 	UIAnimatorEvent ev = animator.gameObject.GetComponent<UIAnimatorEvent>();
	// 	ev.OnAnimationEnter();
	// }

	// public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	// {
	// 	Debug.LogError("ok exit state");
	// 	UIAnimatorEvent ev = animator.gameObject.GetComponent<UIAnimatorEvent>();
	// 	ev.OnAnimationExit();
	// }

	//  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	// override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// 	UIAnimatorEvent ev = animator.gameObject.GetComponent<UIAnimatorEvent>();
	// 	ev.OnAnimationEnter();
	// }

	// // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	// override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// 	Debug.LogError("on state update");
	// }

	// // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	// override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// 	UIAnimatorEvent ev = animator.gameObject.GetComponent<UIAnimatorEvent>();
	// 	ev.OnAnimationExit();
	// }

	// // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	// override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	// {
	// 	Debug.Log("on state move");
	// }

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
