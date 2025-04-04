using System.Collections;
using Assets.Scripts.Utils;
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
        if (PlayerInventory.Instance.HasItem(requiredKey)) {
            if (showUnlockMessage) {
                GameUI.Tooltip.Show(
                    customUnlockMessage.Length > 0 ?
                    LocalizationHelper.LocalizeTooltip(customUnlockMessage)
                    :
                    LocalizationHelper.LocalizeTooltip("Used", requiredKey.itemName)
                    );
            }
            if (removeKeyOnOpen) {
                PlayerInventory.Instance.RemoveItem(requiredKey);
            }

            lockedOnKey = false;
            state = DoorStateEnum.Opened;
            SaveState();
            ToggleOpening();
        } else {
            DoorAudio.Instance.PlayLocked(transform.position);
            GameUI.Tooltip.Show(LocalizationHelper.LocalizeTooltip(lockedMessage));
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
    public void CloseDoorFast(bool silentMode = false) {
        if (isOpened && !isOpeningOrClosingState) {
            StartCoroutine(CloseDoorFastRoutine(silentMode));
        }
    }

    private IEnumerator CloseDoorFastRoutine(bool silentMode) {
        float defaultOpenSpeed = openSpeed;
        openSpeed *= 2f;
        ToggleOpening(silentMode);
        yield return new WaitForSeconds(.5f);
        openSpeed = defaultOpenSpeed;
    }
}
