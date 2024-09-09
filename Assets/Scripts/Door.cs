using UnityEngine;

public class Door : Interactable {
    private const string OPEN_MESSAGE = "Open door";
    private const string CLOSE_MESSAGE = "Close door";
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private float openSpeed = 150f;
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

    private bool isOpened;
    private bool openingDoor;
    private bool isOpeningState = false;
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
        if (isOpeningState) {
            return;
        }
        isOpeningState = true;
        openingDoor = !openingDoor;
        if (isOpened) {
            interactMessage = OPEN_MESSAGE;
        } else {
            interactMessage = CLOSE_MESSAGE;
        }
    }
    private void HandleOpen() {
        if (!isOpeningState) {
            return;
        }
        if (openingDoor && currentAngle < maxOpenAngle) {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = maxOpenAngle - currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = true;
                isOpeningState = false;
            }
            transform.RotateAround(rotationPoint.position, transform.up, openForward ? angleToRotate : -angleToRotate);
            currentAngle += angleToRotate;
        } else {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = false;
                isOpeningState = false;
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

            ToggleOpening();
            lockedOnKey = false;
        } else {
            TooltipUI.Instance.Show("Locked");
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
                TooltipUI.Instance.Show("Locked from the other side");
            }
        }
    }
}
