using UnityEngine;

public class DoorToAnotherLevel : Interactable {
    [Header("UI")]
    [SerializeField] private string lockedMessage = "Locked";
    [Header("Settings")]
    [SerializeField] private bool lockedOnKey;
    [SerializeField] private ItemData requiredKey;
    private readonly string openMessage = "Open";

    private void Awake() {
        interactMessage = openMessage;
    }
    protected override void Interact() {
        if (lockedOnKey) {
            TryOpen();
            return;
        }
        OpenDoor();
    }
    private void TryOpen() {
        ItemData itemFound = PlayerInventory.Instance.items.Find(item => item == requiredKey);
        if (itemFound) {
            TooltipUI.Instance.Show($"Used {itemFound.itemName}");
            PlayerInventory.Instance.RemoveItem(itemFound);

            OpenDoor();
            lockedOnKey = false;
        } else {
            DoorAudio.Instance.PlayLocked(transform.position);
            TooltipUI.Instance.Show(lockedMessage);
        }
    }
    private void OpenDoor() {
        Debug.Log("Opening door");
        DoorAudio.Instance.PlayOpen(transform.position);
    }
}

