using System;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    private enum AnimationBools {
        Idle,
        Walk,
        Run,
    }
    [SerializeField] private Animator animator;

    public void Idle() {
        ClearAnimations(AnimationBools.Idle);
        ActivateBoolAnimation(AnimationBools.Idle);
    }
    public void Walk() {
        ClearAnimations(AnimationBools.Walk);
        ActivateBoolAnimation(AnimationBools.Walk);
    }
    public void Run() {
        ClearAnimations(AnimationBools.Run);
        ActivateBoolAnimation(AnimationBools.Run);
    }
    private void ClearAnimations(AnimationBools currentAnimation) {
        foreach (AnimationBools trigger in Enum.GetValues(typeof(AnimationBools))) {
            if (trigger != currentAnimation) {
                animator.SetBool(trigger.ToString(), false);
            }
        }
    }
    private void ActivateBoolAnimation(AnimationBools animation) {
        animator.SetBool(animation.ToString(), true);
    }
}
