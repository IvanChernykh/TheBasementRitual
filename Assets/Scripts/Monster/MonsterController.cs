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
    [SerializeField] private MonsterAnimation animationController;
    [Header("Sounds")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] randomRoars;
    [SerializeField] private float footstepWalkTimerMax = .6f;
    [SerializeField] private float footstepRunTimerMax = .3f;
    [SerializeField] private float footstepVolume = 0.1f;
    [SerializeField] private float roarVolume = 0.5f;
    private float roarIntervalMin = 6f;
    private float roarIntervalMax = 30f;
    private float timeUntilNextRoar;
    private float roarTimer;
    private float footstepTimer;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float fieldOfViewAngle = 90f;
    [SerializeField] private float hearingDistance = 1f;
    [SerializeField] private float sightDistance = 20f;

    [Header("Patrol State")]
    [SerializeField] private Transform[] patrolPoints;

    // patrol state
    private int nextPointIdx = 0;
    private readonly float arrivalPointDistance = 0.2f;

    // chase state
    private Vector3 playerLastSeenPos;

    // search state
    private List<Vector3> searchPoints = new List<Vector3>();
    private bool searchPointsGenerated = false;

    private void Start() {
        StartPatrolling();
        timeUntilNextRoar = Random.Range(roarIntervalMin, roarIntervalMax);
    }
    private void Update() {
        PlayRandomRoar();
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
        // todo: remove it later
        DebugLogs();
    }
    // state handlers
    private void HandlePatrol() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        if (agent.remainingDistance < arrivalPointDistance) {
            nextPointIdx = Random.Range(0, patrolPoints.Length);
        }
        agent.SetDestination(patrolPoints[nextPointIdx].position);
        PlayWalkFootstep();
    }
    private void HandleChasePlayer() {
        if (!CanSeePlayer()) {
            StartInvestigatingLastSeenPlayerPosition();
            return;
        }
        agent.SetDestination(PlayerController.Instance.transform.position);
        PlayRunFootstep();
    }
    private void HandleInvestigateLastSeenPlayerPos() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        agent.SetDestination(playerLastSeenPos);
        if (agent.remainingDistance < arrivalPointDistance) {
            StartSearchingPlayer();
            return;
        }
    }
    private void HandleSearchPlayer() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        if (agent.remainingDistance < arrivalPointDistance) {
            if (!searchPointsGenerated) {
                searchPoints = GetNearestPoints(
                    playerLastSeenPos,
                    PlayerController.Instance.transform.position,
                    patrolPoints,
                    pointCount: 3,
                    exclusionRadius: 1f
                );
                searchPointsGenerated = true;
            }
            if (searchPoints.Count > 0) {
                Vector3 nextSearchPoint = searchPoints[0];
                agent.SetDestination(nextSearchPoint);

                searchPoints.RemoveAt(0);
            } else {
                searchPointsGenerated = false;
                StartPatrolling();
            }
        }
    }
    // state transitions
    private void StartPatrolling() {
        agent.speed = walkSpeed;
        animationController.Walk();
        currentState = State.Patrolling;
    }
    private void StartChasingPlayer() {
        // animationController.Run();
        agent.speed = runSpeed;
        currentState = State.ChasingPlayer;
    }
    private void StartInvestigatingLastSeenPlayerPosition() {
        playerLastSeenPos = PlayerController.Instance.transform.position;
        // animationController.Idle();
        currentState = State.InvestigatingLastSeenPlayerPosition;
    }
    private void StartSearchingPlayer() {
        agent.speed = walkSpeed;
        animationController.Walk();
        currentState = State.SearchingPlayer;
    }
    // other
    private bool CanSeePlayer() {
        Vector3 offsetY = Vector3.up;
        Vector3 eyePosition = transform.position + offsetY;
        float distanceToPlayer = DistanceToPlayer(eyePosition);

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
    private List<Vector3> GetNearestPoints(
        Vector3 origin,
        Vector3 playerCurrentPosition,
        Transform[] searchPoints,
        int pointCount,
        float exclusionRadius
        ) {
        Vector3 directionToPlayer = (playerCurrentPosition - origin).normalized;

        List<Vector3> nearestPoints = new List<Vector3>();
        List<Vector3> sortedPoints = searchPoints
            .OrderBy(p => Vector3.Distance(origin, p.position))
            .Select(p => p.position)
            .ToList();

        foreach (Vector3 point in sortedPoints) {
            Vector3 directionToPoint = (point - origin).normalized;

            // if point is on the same side as playerCurrentPosition with a certain angle
            if (Vector3.Dot(directionToPlayer, directionToPoint) > Mathf.Cos(Mathf.Deg2Rad * 45f)) // 90 deg
            {
                if (Vector3.Distance(origin, point) > exclusionRadius) {
                    nearestPoints.Add(point);
                }
            }
            if (nearestPoints.Count == pointCount) {
                break;
            }
        }

        return nearestPoints;
    }
    private float DistanceToPlayer(Vector3 origin) {
        return Vector3.Distance(origin, PlayerController.Instance.transform.position);
    }
    private void PlayWalkFootstep() {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0) {
            footstepTimer = footstepWalkTimerMax;
            SoundManager.Instance.PlaySound(footstepSounds, transform.position, footstepVolume, maxDistance: 10f, rolloffMode: AudioRolloffMode.Custom);
        }
    }
    private void PlayRunFootstep() {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0) {
            footstepTimer = footstepRunTimerMax;
            SoundManager.Instance.PlaySound(footstepSounds, transform.position, footstepVolume, maxDistance: 10f, rolloffMode: AudioRolloffMode.Custom);
        }
    }
    private void PlayRandomRoar() {
        roarTimer += Time.deltaTime;
        if (roarTimer >= timeUntilNextRoar) {
            SoundManager.Instance.PlaySound(randomRoars, transform.position, roarVolume, maxDistance: 20f);

            timeUntilNextRoar = Random.Range(roarIntervalMin, roarIntervalMax);
            roarTimer = 0f;
        }
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
    private void DebugLogs() {
        Debug.Log(currentState);
    }
}
