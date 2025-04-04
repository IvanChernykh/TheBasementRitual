using Assets.Scripts.Utils;
using UnityEngine;

public class DoorDouble : DoorBase {
    [Header("Settings")]
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;
    [SerializeField] private Transform rotationPointLeft;
    [SerializeField] private Transform rotationPointRight;
    [SerializeField] private float openSpeed = 180f;

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
    protected override void ToggleOpening(bool _ = true) {
        if (isOpeningOrClosingState) {
            return;
        }
        isOpeningOrClosingState = true;
        openingDoor = !openingDoor;
        if (isOpened) {
            DoorAudio.Instance.PlayCloseDoubleDoor(transform.position);
            interactMessage = openMessage;
        } else {
            DoorAudio.Instance.PlayOpen(transform.position);
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
            doorLeft.RotateAround(rotationPointLeft.position, doorLeft.up, angleToRotate);
            doorRight.RotateAround(rotationPointRight.position, doorLeft.up, -angleToRotate);
            currentAngle += angleToRotate;
        } else {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = false;
                isOpeningOrClosingState = false;
            }
            doorLeft.RotateAround(rotationPointLeft.position, doorLeft.up, -angleToRotate);
            doorRight.RotateAround(rotationPointRight.position, doorLeft.up, angleToRotate);
            currentAngle -= angleToRotate;
        }

    }
    private void TryOpen() {
        if (PlayerInventory.Instance.HasItem(requiredKey)) {
            GameUI.Tooltip.Show(LocalizationHelper.LocalizeTooltip("Used", requiredKey.itemName));
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
}
