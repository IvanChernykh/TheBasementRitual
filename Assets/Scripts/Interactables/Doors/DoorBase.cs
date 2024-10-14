using UnityEngine;
using Assets.Scripts.Utils;
using System;

public enum DoorStateEnum {
    Default,
    Opened,
    Locked,
    LockedFromTheOtherSide,
}
public class DoorBase : Interactable {
    [Header("Messages")]
    [SerializeField] protected string lockedMessage = "Locked";
    protected string openMessage = "Open";
    protected string closeMessage = "Close";

    [Header("Locked State")]
    [SerializeField] protected bool lockedOnKey;
    [SerializeField] protected bool lockedFromOtherSide;
    [SerializeField] protected bool lockedFromBehindSide;
    [SerializeField] protected bool removeKeyOnOpen = true;
    [SerializeField] protected ItemData requiredKey;


    [Header("State Saving")]
    [SerializeField] protected bool saveState;
    [SerializeField] protected string doorId;
    protected DoorStateEnum state = DoorStateEnum.Default;
    public string Id { get => doorId; }

    public bool isOpened { get; protected set; }
    public bool isLocked { get => lockedOnKey; }
    public bool isOpeningOrClosingState { get; protected set; } = false;
    protected bool openingDoor; // opening or closing

    protected float maxOpenAngle = 90f;
    protected float currentAngle = 0f;

    private void Awake() {
        interactMessage = openMessage;
    }

    protected override void Interact() {
        throw new NotImplementedException();
    }
    protected virtual void ToggleOpening(bool silentMode = false) { }


    protected void TryOpenLockedFromOtherSide() {
        if (PlayerUtils.DistanceToPlayer(transform.position) < PlayerController.Instance.interactDistance * 1.5) {
            Vector3 directionToPlayer = PlayerUtils.DirectionToPlayer(transform.position);
            directionToPlayer.y = 0;

            Vector3 doorForward = lockedFromBehindSide ? -transform.forward : transform.forward;

            float dotProduct = Vector3.Dot(doorForward, directionToPlayer.normalized);

            if (dotProduct > 0) {
                lockedFromOtherSide = false;
                state = DoorStateEnum.Opened;
                SaveState();
                ToggleOpening();
            } else {
                DoorAudio.Instance.PlayLocked(transform.position);
                TooltipUI.Instance.Show("Locked from the other side");
            }
        }
    }
    public void OpenDoor() {
        if (!isOpened && !isOpeningOrClosingState) {
            lockedOnKey = false;
            state = DoorStateEnum.Opened;
            SaveState();
            ToggleOpening();
        }
    }

    protected void SaveState() {
        if (saveState) {
            SceneStateManager.Instance.AddOrUpdateDoorState(new DoorState(id: doorId, lockedMessage: lockedMessage, state: state.ToString()));
        }
    }
    public void SetState(DoorState doorState) {
        lockedMessage = doorState.lockedMessage;

        if (Enum.TryParse(doorState.state, out DoorStateEnum parsedState)) {
            switch (parsedState) {
                case DoorStateEnum.Opened:
                    lockedOnKey = false;
                    lockedFromOtherSide = false;
                    break;
                case DoorStateEnum.Locked:
                    lockedOnKey = true;
                    break;
                case DoorStateEnum.LockedFromTheOtherSide:
                    lockedFromOtherSide = true;
                    break;
            }
        }
    }
}
