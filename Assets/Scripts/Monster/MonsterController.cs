using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour {
    public enum State {
        Patrolling,
        ChasingPlayer,
        InvestigatingLastSeenPlayerPosition,
        SearchingPlayer,
    }
    private State currentState = State.Patrolling;

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private MonsterAnimation monsterAnimation;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 3.8f;
    [SerializeField] private float fieldOfViewAngle = 90f;
    [SerializeField] private float hearingDistance = 1f;
    [SerializeField] private float sightDistance = 20f;

    [Header("Patrol State")]
    [SerializeField] private Transform[] patrolPoints;
    private int nextPointIdx = 0;
    private readonly float arrivalPointDistance = 0.2f;
    private Vector3 playerLastSeenPos;

    // Search State
    private List<Vector3> searchPoints = new List<Vector3>();
    private bool searchPointsGenerated = false;

    private void Start() {
        agent.speed = walkSpeed;
    }
    private void Update() {
        Debug.Log(currentState);
        switch (currentState) {
            case State.Patrolling:
                HandlePatrol();
                break;
            case State.ChasingPlayer:
                HandleChasePlayer();
                break;
            case State.InvestigatingLastSeenPlayerPosition:
                HandleInvestigateLastSeenPlayerPos();
                break;
            case State.SearchingPlayer:
                HandleSearchPlayer();
                break;
        }
    }
    // state handlers
    private void HandlePatrol() {
        if (CanSeePlayer()) {
            currentState = State.ChasingPlayer;
            return;
        }
        agent.speed = walkSpeed;
        if (agent.remainingDistance < arrivalPointDistance) {
            nextPointIdx = Random.Range(0, patrolPoints.Length);
        }
        agent.SetDestination(patrolPoints[nextPointIdx].position);
    }
    private void HandleChasePlayer() {
        agent.speed = runSpeed;
        if (!CanSeePlayer()) {
            playerLastSeenPos = PlayerController.Instance.transform.position;
            currentState = State.InvestigatingLastSeenPlayerPosition;
            return;
        }
        transform.LookAt(PlayerController.Instance.transform);
        agent.SetDestination(PlayerController.Instance.transform.position);
    }
    private void HandleInvestigateLastSeenPlayerPos() {
        if (CanSeePlayer()) {
            currentState = State.ChasingPlayer;
            return;
        }
        agent.SetDestination(playerLastSeenPos);
        if (agent.remainingDistance < arrivalPointDistance) {
            currentState = State.SearchingPlayer;
            return;
        }
    }
    private void HandleSearchPlayer() {
        if (CanSeePlayer()) {
            currentState = State.ChasingPlayer;
            return;
        }
        agent.speed = walkSpeed;
        if (agent.remainingDistance < arrivalPointDistance) {
            if (!searchPointsGenerated) {
                searchPoints = GetNearestPoints(playerLastSeenPos, patrolPoints, count: 3, exclusionRadius: 1f);
                searchPointsGenerated = true;
            }
            if (searchPoints.Count > 0) {
                Vector3 nextSearchPoint = searchPoints[0];
                agent.SetDestination(nextSearchPoint);

                searchPoints.RemoveAt(0);
            } else {
                searchPointsGenerated = false;
                currentState = State.Patrolling;
            }
        }
    }
    // other
    private bool CanSeePlayer() {
        Vector3 offsetY = Vector3.up;
        Vector3 eyePosition = transform.position + offsetY;
        float distanceToPlayer = Vector3.Distance(eyePosition, PlayerController.Instance.transform.position);

        if (distanceToPlayer > sightDistance) {
            return false;
        }
        if (distanceToPlayer <= hearingDistance) {
            return true;
        }

        Vector3 directionToPlayer = (PlayerController.Instance.transform.position - eyePosition).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= fieldOfViewAngle / 2f) {
            if (Physics.Raycast(eyePosition, directionToPlayer, out RaycastHit hit, distanceToPlayer)) {
                if ((1 << hit.collider.gameObject.layer) == playerLayerMask) {
                    return true;
                }
            }
        }
        return false;
    }
    private List<Vector3> GetNearestPoints(Vector3 origin, Transform[] points, int count, float exclusionRadius) {
        List<Vector3> nearestPoints = new List<Vector3>();
        List<Vector3> sortedPoints = points
        .OrderBy(p => Vector3.Distance(origin, p.position))
        .Select(p => p.position)
        .ToList();

        foreach (Vector3 point in sortedPoints) {
            if (Vector3.Distance(origin, point) > exclusionRadius) {
                nearestPoints.Add(point);
            }
            if (nearestPoints.Count == count) {
                break;
            }
        }
        return nearestPoints;
    }
    // debug
    private void OnDrawGizmos() {
        Vector3 offsetY = Vector3.up;
        Vector3 eyePosition = transform.position + offsetY;

        Gizmos.color = Color.green;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;

        Gizmos.DrawLine(eyePosition, eyePosition + leftBoundary * sightDistance);
        Gizmos.DrawLine(eyePosition, eyePosition + rightBoundary * sightDistance);
        Gizmos.DrawLine(eyePosition, eyePosition + transform.forward * sightDistance);
        Gizmos.DrawWireSphere(eyePosition, hearingDistance);
    }
}
