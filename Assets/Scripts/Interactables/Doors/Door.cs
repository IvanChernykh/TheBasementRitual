using UnityEngine;


public class Door : DoorBase {
    [Header("Opening")]
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private float openSpeed = 200f;
    [SerializeField] private bool openForward;

    [Header("Tooltip")]
    [Tooltip("Show tooltip message after door unlocked with key")]
    [SerializeField] private bool showUnlockMessage = true;
    [SerializeField] private string customUnlockMessage;
    // [SerializeField] private string customKeyLockedMessage;



    private void Update() {
        HandleOpen();
    }
    protected override void Interact() {
        if (lockedFromOtherSide) {
            TryOpenLockedFromOtherSide();
            return;
        }
        if (lockedOnKey) {
            TryOpen();
            return;
        }
        ToggleOpening();
    }
    protected override void ToggleOpening(bool silentMode = false) {
        if (isOpeningOrClosingState) {
            return;
        }
        isOpeningOrClosingState = true;
        openingDoor = !openingDoor;
        if (isOpened) {
            if (!silentMode) {
                DoorAudio.Instance.PlayClose(transform.position);
            }
            interactMessage = openMessage;
        } else {
            if (!silentMode) {
                DoorAudio.Instance.PlayOpen(transform.position);
            }
            interactMessage = closeMessage;
        }
    }
    private void HandleOpen() {
        if (!isOpeningOrClosingState) {
            return;
        }
        if (openingDoor && currentAngle < maxOpenAngle) {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = maxOpenAngle - currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = true;
                isOpeningOrClosingState = false;
            }
            transform.RotateAround(rotationPoint.position, transform.up, openForward ? angleToRotate : -angleToRotate);
            currentAngle += angleToRotate;
        } else {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = false;
                isOpeningOrClosingState = false;
            }
            transform.RotateAround(rotationPoint.position, transform.up, openForward ? -angleToRotate : angleToRotate);
            currentAngle -= angleToRotate;
        }
    }
    private void TryOpen() {
        ItemData itemFound = PlayerInventory.Instance.items.Find(item => item == requiredKey);
        if (itemFound) {
            if (showUnlockMessage) {
                TooltipUI.Instance.Show(customUnlockMessage.Length > 0 ? customUnlockMessage : $"Used {itemFound.itemName}");
            }
            if (removeKeyOnOpen) {
                PlayerInventory.Instance.RemoveItem(itemFound);
            }

            lockedOnKey = false;
            state = DoorStateEnum.Opened;
            SaveState();
            ToggleOpening();
        } else {
            DoorAudio.Instance.PlayLocked(transform.position);
            TooltipUI.Instance.Show(lockedMessage);
            // if (customKeyLockedMessage.Length > 0) {
            //     TooltipUI.Instance.Show(customKeyLockedMessage);
            // } else {
            //     DoorAudio.Instance.PlayLocked(transform.position);
            //     TooltipUI.Instance.Show(lockedMessage);
            // }
        }
    }

    public void CloseDoor() {
        if (isOpened && !isOpeningOrClosingState) {
            ToggleOpening();
        }
    }
    public void OpenDoorSilent() {
        if (!isOpened && !isOpeningOrClosingState) {
            lockedOnKey = false;
            state = DoorStateEnum.Opened;
            ToggleOpening(silentMode: true);
            SaveState();
        }
    }
    public void Lock(string lockedMessage = "") {
        lockedOnKey = true;
        if (lockedMessage.Length > 0) {
            this.lockedMessage = lockedMessage;
        }
        state = DoorStateEnum.Locked;
        SaveState();
    }
    public void Unlock() {
        if (lockedOnKey) {
            lockedOnKey = false;
            state = DoorStateEnum.Opened;
            SaveState();
        }
    }

}
