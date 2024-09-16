using System;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    enum AnimationTriggers {
        Idle,
        IdleTurnHead,
        Walk,
        Run
    }
    [SerializeField] private Animator animator;

    public void Idle() {
        ClearAnimations(AnimationTriggers.Idle);
        animator.SetBool(AnimationTriggers.Idle.ToString(), true);
    }
    public void IdleTurnHead() {
        ClearAnimations(AnimationTriggers.IdleTurnHead);
        animator.SetTrigger(AnimationTriggers.IdleTurnHead.ToString());
    }
    public void Walk() {
        ClearAnimations(AnimationTriggers.Walk);
        animator.SetTrigger(AnimationTriggers.Walk.ToString());
    }
    public void Run() {
        ClearAnimations(AnimationTriggers.Run);
        animator.SetTrigger(AnimationTriggers.Run.ToString());
    }
    private void ClearAnimations(AnimationTriggers currentAnimation) {
        foreach (AnimationTriggers trigger in Enum.GetValues(typeof(AnimationTriggers))) {
            if (trigger != currentAnimation) {
                animator.SetBool(trigger.ToString(), false);
            }
        }
    }
}
