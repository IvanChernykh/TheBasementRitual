using UnityEngine;

public class Door : Interactable {
    private const string OPEN_MESSAGE = "Open door";
    private const string CLOSE_MESSAGE = "Close door";
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private float openSpeed = 200f;
    [SerializeField] private bool openForward;
    [Header("Locked State")]
    [SerializeField] private bool lockedFromOtherSide;
    [SerializeField] private bool lockedFromBehindSide;
    [SerializeField] private bool lockedOnKey;
    [SerializeField] private bool removeKeyOnOpen = true;
    [SerializeField] private ItemData requiredKey;
    [Header("Tooltip")]
    [Tooltip("Show tooltip message after door unlocked with key")]
    [SerializeField] private bool showUnlockMessage = true;
    [SerializeField] private string customKeyLockedMessage;

    private bool isOpened;
    private bool openingDoor; // opening or closing
    private bool isOpeningOrClosingState = false;
    private float maxOpenAngle = 90f;
    private float currentAngle = 0f;

    private void Awake() {
        interactMessage = OPEN_MESSAGE;
    }
    private void Update() {
        HandleOpen();
    }
    public override void Interact() {
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
    private void ToggleOpening() {
        if (isOpeningOrClosingState) {
            return;
        }
        isOpeningOrClosingState = true;
        openingDoor = !openingDoor;
        if (isOpened) {
            DoorAudio.Instance.PlayClose(transform.position);
            interactMessage = OPEN_MESSAGE;
        } else {
            DoorAudio.Instance.PlayOpen(transform.position);
            interactMessage = CLOSE_MESSAGE;
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
                TooltipUI.Instance.Show($"Used {itemFound.itemName}");
            }
            if (removeKeyOnOpen) {
                PlayerInventory.Instance.RemoveItem(itemFound);
            }

            lockedOnKey = false;
            ToggleOpening();
        } else {
            if (customKeyLockedMessage.Length > 0) {
                TooltipUI.Instance.Show(customKeyLockedMessage);
            } else {
                DoorAudio.Instance.PlayLocked(transform.position);
                TooltipUI.Instance.Show("Locked");
            }
        }
    }
    private void TryOpenLockedFromOtherSide() {
        Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
        if (Vector3.Distance(playerPos, transform.position) < PlayerController.Instance.interactDistance * 1.5) {
            Vector3 directionToPlayer = playerPos - transform.position;
            directionToPlayer.y = 0;

            Vector3 doorForward = lockedFromBehindSide ? -transform.forward : transform.forward;

            float dotProduct = Vector3.Dot(doorForward, directionToPlayer.normalized);

            if (dotProduct > 0) {
                lockedFromOtherSide = false;
                ToggleOpening();
            } else {
                DoorAudio.Instance.PlayLocked(transform.position);
                TooltipUI.Instance.Show("Locked from the other side");
            }
        }
    }
    public void CloseDoor() {
        if (isOpened && !isOpeningOrClosingState) {
            ToggleOpening();
        }
    }
    public void OpenDoor() {
        if (!isOpened && !isOpeningOrClosingState) {
            ToggleOpening();
        }
    }
    public void Unlock() {
        if (lockedOnKey) {
            lockedOnKey = false;
            ToggleOpening();
        }
    }
}
