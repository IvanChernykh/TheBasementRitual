using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToAnotherLevel : Interactable {
    [Header("UI")]
    [SerializeField] private string lockedMessage = "Locked";
    [Header("Settings")]
    [SerializeField] private bool lockedOnKey;
    [SerializeField] private ItemData requiredKey;
    [SerializeField] private GameScenes sceneToLoad;
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
        } else {
            DoorAudio.Instance.PlayLocked(transform.position);
            TooltipUI.Instance.Show(lockedMessage);
        }
    }
    private void OpenDoor() {
        // Debug.Log("Opening door");
        // DoorAudio.Instance.PlayOpen(transform.position);
        SceneManager.LoadScene((int)sceneToLoad);
    }
}

