using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour {
    public enum State {
        Patrolling,
        ChasingPlayer,
        SearchingPlayer,
    }
    private State currentState = State.Patrolling;

    [Header("Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private MonsterAnimation monsterAnimation;

    [Header("Patrol State")]
    [SerializeField] private Transform[] patrolPoints;
    private int nextPointIdx = 0;
    private bool one;

    private void Update() {
        switch (currentState) {
            case State.Patrolling:
                HandlePatrol();
                break;
            case State.ChasingPlayer:
                break;
            case State.SearchingPlayer:
                break;
        }
    }
    private void HandlePatrol() {
        if (!one) {
            monsterAnimation.Run();
        }
        if (agent.remainingDistance < 0.2f) {
            nextPointIdx = Random.Range(0, patrolPoints.Length);
        }
        agent.SetDestination(patrolPoints[nextPointIdx].position);
    }
}
