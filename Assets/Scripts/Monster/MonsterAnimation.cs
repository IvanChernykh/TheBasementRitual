using System;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    private enum AnimationBools {
        Idle,
        Walk,
        RunOld,
        Run,
    }
    private enum AnimationTriggers {
        Attack
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
    public void RunOld() {
        ClearAnimations(AnimationBools.RunOld);
        ActivateBoolAnimation(AnimationBools.RunOld);
    }
    public void Run() {
        ClearAnimations(AnimationBools.Run);
        ActivateBoolAnimation(AnimationBools.Run);
    }
    public void Attack() {
        animator.SetTrigger(AnimationTriggers.Attack.ToString());
    }
    private void ClearAnimations(AnimationBools currentAnimation) {
        foreach (AnimationBools aBool in Enum.GetValues(typeof(AnimationBools))) {
            if (aBool != currentAnimation) {
                animator.SetBool(aBool.ToString(), false);
            }
        }
    }
    private void ActivateBoolAnimation(AnimationBools animation) {
        animator.SetBool(animation.ToString(), true);
    }
}
