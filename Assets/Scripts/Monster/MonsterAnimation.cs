using UnityEngine;

public class MonsterAnimation : MonoBehaviour {
    enum AnimationTriggers {
        Idle,
        IdleTurnHead,
        Run
    }
    [SerializeField] private Animator animator;

    public void Idle() {
        animator.SetTrigger(AnimationTriggers.Idle.ToString());
    }
    public void IdleTurnHead() {
        animator.SetTrigger(AnimationTriggers.IdleTurnHead.ToString());
    }
    public void Run() {
        animator.SetTrigger(AnimationTriggers.Run.ToString());
    }
}
