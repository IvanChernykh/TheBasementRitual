using System;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    enum AnimationTriggers {
        Idle,
        IdleTurnHead,
        Walk,
        Run,
        Crawl
    }
    [SerializeField] private Animator animator;

    public void Idle() {
        ClearAnimations(AnimationTriggers.Idle);
        ActivateBoolAnimation(AnimationTriggers.Idle);
    }
    public void IdleTurnHead() {
        ClearAnimations(AnimationTriggers.IdleTurnHead);
        ActivateBoolAnimation(AnimationTriggers.IdleTurnHead);
    }
    public void Walk() {
        ClearAnimations(AnimationTriggers.Walk);
        ActivateBoolAnimation(AnimationTriggers.Walk);
    }
    public void Run() {
        ClearAnimations(AnimationTriggers.Run);
        ActivateBoolAnimation(AnimationTriggers.Run);
    }
    public void Crawl() {
        ClearAnimations(AnimationTriggers.Crawl);
        ActivateBoolAnimation(AnimationTriggers.Crawl);
    }
    private void ClearAnimations(AnimationTriggers currentAnimation) {
        foreach (AnimationTriggers trigger in Enum.GetValues(typeof(AnimationTriggers))) {
            if (trigger != currentAnimation) {
                animator.SetBool(trigger.ToString(), false);
            }
        }
    }
    private void ActivateBoolAnimation(AnimationTriggers animation) {
        animator.SetBool(animation.ToString(), true);
    }
}
