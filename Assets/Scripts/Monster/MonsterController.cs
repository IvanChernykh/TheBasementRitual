using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    public enum State {
        Patrolling,
        ChasingPlayer,
        InvestigatingLastSeenPlayerPosition,
        SearchingPlayer,
    }
    private State currentState = State.Patrolling;

    [Header("Core")]
    [SerializeField] private MonsterCore monster;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float fieldOfViewAngle = 100f;
    [SerializeField] private float hearingDistance = 1f;
    [SerializeField] private float sightDistance = 12f;

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
    private PlayerController player;

    private void Start() {
        player = PlayerController.Instance;
        gameObject.SetActive(false);
    }
    private void Update() {
        monster.Sounds.PlayRandomRoar();
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
    // init
    public void Init() {
        StartPatrolling();
        monster.Sounds.PlayRandomRoarImmediately(25f);
    }
    // state handlers
    private void HandlePatrol() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        if (monster.Agent.remainingDistance < arrivalPointDistance) {
            nextPointIdx = Random.Range(0, patrolPoints.Length);
        }
        monster.Agent.SetDestination(patrolPoints[nextPointIdx].position);
        monster.Sounds.PlayWalkFootstep();
    }
    private void HandleChasePlayer() {
        if (!CanSeePlayer()) {
            StartInvestigatingLastSeenPlayerPosition();
            return;
        }
        monster.Agent.SetDestination(player.transform.position);
        monster.Sounds.PlayRunFootstep();
    }
    private void HandleInvestigateLastSeenPlayerPos() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        monster.Agent.SetDestination(playerLastSeenPos);
        if (monster.Agent.remainingDistance < arrivalPointDistance) {
            StartSearchingPlayer();
            return;
        }
    }
    private void HandleSearchPlayer() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        if (monster.Agent.remainingDistance < arrivalPointDistance) {
            if (!searchPointsGenerated) {
                searchPoints = GetNearestPoints(
                    playerLastSeenPos,
                    patrolPoints,
                    pointCount: 3,
                    exclusionRadius: 1f
                );
                searchPointsGenerated = true;
            }
            if (searchPoints.Count > 0) {
                Vector3 nextSearchPoint = searchPoints[0];
                monster.Agent.SetDestination(nextSearchPoint);

                searchPoints.RemoveAt(0);
            } else {
                searchPointsGenerated = false;
                StartPatrolling();
            }
        }
    }
    // state transitions
    private void StartPatrolling() {
        monster.Agent.speed = walkSpeed;
        monster.Animation.Walk();
        currentState = State.Patrolling;
    }
    private void StartChasingPlayer() {
        monster.Animation.Run();
        monster.Agent.speed = runSpeed;
        currentState = State.ChasingPlayer;
        StartChaseMusic();
    }
    private void StartInvestigatingLastSeenPlayerPosition() {
        playerLastSeenPos = player.transform.position;
        currentState = State.InvestigatingLastSeenPlayerPosition;
    }
    private void StartSearchingPlayer() {
        StopChaseMusic();
        monster.Agent.speed = walkSpeed;
        monster.Animation.Walk();
        currentState = State.SearchingPlayer;
    }
    // other
    private bool CanSeePlayer() {
        if (player.isHiding) {
            return false;
        }
        Vector3 offsetY = Vector3.up;
        Vector3 eyePosition = transform.position + offsetY;
        float distanceToPlayer = PlayerUtils.DistanceToPlayer(eyePosition);
        float currentSightDistance = player.isCrouching ? sightDistance * .6f : sightDistance;

        if (distanceToPlayer > currentSightDistance) {
            return false;
        }
        if (distanceToPlayer <= hearingDistance) {
            return true;
        }

        Vector3 directionToPlayer = PlayerUtils.DirectionToPlayerNormalized(eyePosition);
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
        Transform[] searchPoints,
        int pointCount,
        float exclusionRadius
        ) {
        Vector3 directionToPlayer = PlayerUtils.DirectionToPlayerNormalized(origin);

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
    // sounds
    private void StartChaseMusic() {
        BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.ChaseMusic, 1f);
        BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.MainAmbient, 1f);
        BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.DeepImpacts, 1f);
    }
    private void StopChaseMusic() {
        BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.ChaseMusic, 2f);
        BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.MainAmbient, 2f);
        BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.DeepImpacts, 1f);
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
