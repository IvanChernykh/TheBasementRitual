using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    public enum State {
        Idle,
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
    [SerializeField] private float runSpeed = 3.8f;
    [SerializeField] private float fieldOfViewDefault = 120f;
    [SerializeField] private float hearingDistance = 2f;
    [SerializeField] private float sightDistance = 12f;

    private float FieldOfViewExpanded;
    private float fieldOfViewCurrent;

    [Header("Navigation Points")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform[] roomPoints;

    // patrol state
    private int nextPointIdx = 0;
    private readonly float arrivalPointDistance = 0.2f;

    // chase state
    private Vector3 playerLastSeenPos;

    // search state
    private List<Vector3> searchPoints = new List<Vector3>();
    private bool searchPointsGenerated = false;
    private readonly float searchPositionTimerMax = 4.7f;
    private float searchPositionTimer;
    private PlayerController player;
    [Header("Door Interaction")]
    [SerializeField] private LayerMask doorLayerMask;
    [SerializeField] private float checkDistance = 2f;

    private void Start() {
        fieldOfViewCurrent = fieldOfViewDefault;
        FieldOfViewExpanded = Mathf.Clamp(fieldOfViewDefault + 40f, fieldOfViewDefault, 260f);

        player = PlayerController.Instance;
        gameObject.SetActive(false);
    }
    private void Update() {
        monster.Sounds.PlayRandomRoar();
        CheckForDoorsAndOpen();
        switch (currentState) {
            case State.Idle:
                HandleIdle();
                break;
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
    private void HandleIdle() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
    }
    private void HandlePatrol() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        if (monster.Agent.remainingDistance < arrivalPointDistance) {
            if (patrolPoints.All(item => PlayerUtils.DistanceToPlayer(item.position) > 21)) {
                nextPointIdx = Random.Range(0, patrolPoints.Length);
            } else {
                do {
                    nextPointIdx = Random.Range(0, patrolPoints.Length);
                    // to avoid getting point that is too far from player
                } while (PlayerUtils.DistanceToPlayer(patrolPoints[nextPointIdx].position) > 21);
            }
        }
        monster.Agent.SetDestination(patrolPoints[nextPointIdx].position);
        monster.Sounds.PlayWalkFootstep();
    }
    private void HandleChasePlayer() {
        if (!CanSeePlayer()) {
            StartInvestigatingLastSeenPlayerPosition();
            return;
        }
        // get to player faster if he is far away
        if (PlayerUtils.DistanceToPlayer(transform.position) > sightDistance / 2) {
            monster.Agent.speed = runSpeed * 1.5f;
        } else {
            monster.Agent.speed = runSpeed;
        }
        if (PlayerUtils.DistanceToPlayer(transform.position) < monster.Attack.AttackDistance) {
            monster.Agent.ResetPath();
        } else {
            monster.Agent.SetDestination(player.transform.position);
        }
        monster.Sounds.PlayRunFootstep();
    }
    private void HandleInvestigateLastSeenPlayerPos() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        monster.Agent.SetDestination(playerLastSeenPos);
        if (monster.Agent.remainingDistance < arrivalPointDistance) {
            if (player.isHiding) {
                if (searchPositionTimer < searchPositionTimerMax) {
                    monster.Animation.Idle();
                    searchPositionTimer += Time.deltaTime;
                    return;
                } else {
                    StartSearchingPlayer();
                    searchPositionTimer = 0;
                }
            } else {
                StartSearchingPlayer();
            }
        }
    }
    private readonly float roomSearchDistance = 5f;
    private void HandleSearchPlayer() {
        if (CanSeePlayer()) {
            StartChasingPlayer();
            return;
        }
        if (monster.Agent.remainingDistance < arrivalPointDistance) {
            if (!searchPointsGenerated) {
                if (roomPoints.Length > 0 && System.Array.Exists(roomPoints, item => PlayerUtils.DistanceToPlayer(item.position) < roomSearchDistance)) {
                    Vector3 point = System.Array.Find(roomPoints, item => PlayerUtils.DistanceToPlayer(item.position) < roomSearchDistance).position;
                    searchPoints = GetRandomPoints(point, 3f);
                    searchPointsGenerated = true;
                } else {
                    searchPoints = GetNearestPointsToPlayer(
                        playerLastSeenPos,
                        patrolPoints,
                        pointCount: 3,
                        exclusionRadius: 1f
                    );
                    searchPointsGenerated = true;
                }
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
    public void StartIdle() {
        StopChaseMusic();
        fieldOfViewCurrent = fieldOfViewDefault;
        monster.Animation.Idle();
        monster.Agent.ResetPath();
        currentState = State.Idle;
    }
    public void StartPatrolling() {
        fieldOfViewCurrent = fieldOfViewDefault;
        monster.Agent.speed = walkSpeed;
        monster.Animation.Walk();
        currentState = State.Patrolling;
    }
    public void StartChasingPlayer() {
        fieldOfViewCurrent = FieldOfViewExpanded;
        monster.Animation.Run();
        currentState = State.ChasingPlayer;
        PlayerController.Instance.SetInChase(true);
        StartChaseMusic();
    }
    private void StartInvestigatingLastSeenPlayerPosition() {
        fieldOfViewCurrent = FieldOfViewExpanded;
        playerLastSeenPos = player.transform.position;
        currentState = State.InvestigatingLastSeenPlayerPosition;
    }
    private void StartSearchingPlayer() {
        StopChaseMusic();
        PlayerController.Instance.SetInChase(false);
        monster.Agent.speed = walkSpeed;
        monster.Animation.Walk();
        currentState = State.SearchingPlayer;
    }
    // other
    private float GetSightDistance() {
        if (player.isCrouching) {
            return sightDistance * .6f;
        }
        if (currentState == State.ChasingPlayer) {
            return sightDistance * 1.5f;
        }
        return sightDistance;
    }
    private bool CanSeePlayer() {
        if (player.isHiding) {
            return false;
        }
        Vector3 offsetY = Vector3.up;
        Vector3 eyePosition = transform.position + offsetY;
        float distanceToPlayer = PlayerUtils.DistanceToPlayer(eyePosition);
        float currentSightDistance = GetSightDistance();

        if (distanceToPlayer > currentSightDistance) {
            return false;
        }
        if (distanceToPlayer <= hearingDistance) {
            return true;
        }

        Vector3 directionToPlayer = PlayerUtils.DirectionToPlayerNormalized(eyePosition);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= fieldOfViewCurrent / 2f) {
            if (Physics.Raycast(eyePosition, directionToPlayer, out RaycastHit hit, distanceToPlayer)) {
                if ((1 << hit.collider.gameObject.layer) == playerLayerMask) {
                    return true;
                }
            }
        }
        return false;
    }

    private void CheckForDoorsAndOpen() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, checkDistance, doorLayerMask)) {
            if (hit.collider.TryGetComponent(out Door door) && !door.isOpened && !door.isOpeningOrClosingState) {
                door.InteractAction();
            }
        }
    }
    private List<Vector3> GetNearestPointsToPlayer(
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
    private List<Vector3> GetRandomPoints(Vector3 origin, float radius, int numberOfPoints = 3) {
        List<Vector3> randomPoints = new List<Vector3>();

        for (int i = 0; i < numberOfPoints; i++) {
            randomPoints.Add(origin + Random.insideUnitSphere * radius);
        }

        return randomPoints;
    }
    // sounds
    private void StartChaseMusic() {
        BackgroundMusic.Instance.PlayChaseMusic(.5f);
        BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.MainAmbient, 1f);
    }
    private void StopChaseMusic() {
        if (BackgroundMusic.Instance.IsMusicPlaying(BackgroundMusic.Sounds.ChaseMusic)) {
            BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.ChaseMusic, 5f);
        }
        if (!BackgroundMusic.Instance.IsMusicPlaying(BackgroundMusic.Sounds.ChaseMusic)) {
            BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.MainAmbient, 2f);
        }
    }
}
