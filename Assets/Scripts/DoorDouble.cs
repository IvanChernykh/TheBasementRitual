using UnityEngine;

public class DoorDouble : Interactable {
    [Header("UI")]
    [SerializeField] private string openMessage = "Open door";
    [SerializeField] private string closeMessage = "Close door";
    [SerializeField] private string lockedMessage = "Locked";
    [Header("Settings")]
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;
    [SerializeField] private Transform rotationPointLeft;
    [SerializeField] private Transform rotationPointRight;
    [SerializeField] private float openSpeed = 180f;
    [SerializeField] private bool lockedOnKey;
    [SerializeField] private bool lockedFromOtherSide;
    [SerializeField] private bool lockedFromBehindSide;
    [SerializeField] private ItemData requiredKey;
    private bool isOpened;
    private bool openingDoor;
    private bool isOpeningState = false;
    private float maxOpenAngle = 90f;
    private float currentAngle = 0f;

    private void Awake() {
        interactMessage = openMessage;
    }
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
    private void ToggleOpening() {
        if (isOpeningState) {
            return;
        }
        isOpeningState = true;
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
            doorLeft.RotateAround(rotationPointLeft.position, doorLeft.up, angleToRotate);
            doorRight.RotateAround(rotationPointRight.position, doorLeft.up, -angleToRotate);
            currentAngle += angleToRotate;
        } else {
            float angleToRotate = openSpeed * Time.deltaTime;
            float remainingAngle = currentAngle;
            if (angleToRotate > remainingAngle) {
                angleToRotate = remainingAngle;
                isOpened = false;
                isOpeningState = false;
            }
            doorLeft.RotateAround(rotationPointLeft.position, doorLeft.up, -angleToRotate);
            doorRight.RotateAround(rotationPointRight.position, doorLeft.up, angleToRotate);
            currentAngle -= angleToRotate;
        }

    }
    private void TryOpen() {
        ItemData itemFound = PlayerInventory.Instance.items.Find(item => item == requiredKey);
        if (itemFound) {
            TooltipUI.Instance.Show($"Used {itemFound.itemName}");
            PlayerInventory.Instance.RemoveItem(itemFound);

            ToggleOpening();
            lockedOnKey = false;
        } else {
            DoorAudio.Instance.PlayLocked(transform.position);
            TooltipUI.Instance.Show(lockedMessage);
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
}
