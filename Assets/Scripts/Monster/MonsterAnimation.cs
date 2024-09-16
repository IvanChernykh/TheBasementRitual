using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    enum AnimationTriggers {
        Idle,
        IdleTurnHead,
        Run
    }
    [SerializeField] private Animator animator;

    public void Idle() {
        ClearAnimations();
        animator.SetBool(AnimationTriggers.Idle.ToString(), true);
    }
    public void IdleTurnHead() {
        ClearAnimations();
        animator.SetTrigger(AnimationTriggers.IdleTurnHead.ToString());
    }
    public void Run() {
        ClearAnimations();
        animator.SetTrigger(AnimationTriggers.Run.ToString());
    }
    private void ClearAnimations() {
        animator.SetBool(AnimationTriggers.Idle.ToString(), false);
        animator.SetBool(AnimationTriggers.IdleTurnHead.ToString(), false);
        animator.SetBool(AnimationTriggers.Run.ToString(), false);
    }
}
