using System;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    public enum AnimationBools {
        Idle,
        Walk,
        RunOld,
        Run,
    }
    public enum AnimationTriggers {
        Attack
    }
    [SerializeField] private Animator animator;
    public string currentAnimation { get; private set; } = "";

    public void Idle() {
        currentAnimation = AnimationBools.Idle.ToString();
        ClearAnimations(AnimationBools.Idle);
        ActivateBoolAnimation(AnimationBools.Idle);
    }
    public void Walk() {
        currentAnimation = AnimationBools.Walk.ToString();
        ClearAnimations(AnimationBools.Walk);
        ActivateBoolAnimation(AnimationBools.Walk);
    }
    public void RunOld() {
        currentAnimation = AnimationBools.RunOld.ToString();
        ClearAnimations(AnimationBools.RunOld);
        ActivateBoolAnimation(AnimationBools.RunOld);
    }
    public void Run() {
        currentAnimation = AnimationBools.Run.ToString();
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
    public void DisableAnimation() {
        animator.enabled = false;
    }
    public void EnableAnimation() {
        animator.enabled = true;
    }
    private void ActivateBoolAnimation(AnimationBools animation) {
        animator.SetBool(animation.ToString(), true);
    }
}
