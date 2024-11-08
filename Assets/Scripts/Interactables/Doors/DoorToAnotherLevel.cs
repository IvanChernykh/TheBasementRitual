using Assets.Scripts.Utils;
using UnityEngine;

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
        if (PlayerInventory.Instance.HasItem(requiredKey)) {
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("Used", requiredKey.itemName));
            PlayerInventory.Instance.RemoveItem(requiredKey);

            OpenDoor();
        } else {
            DoorAudio.Instance.PlayLocked(transform.position);
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip(lockedMessage));
        }
    }
    private void OpenDoor() {
        if (SceneController.Instance != null) {
            SceneController.Instance.LoadNextLevel(sceneToLoad);
        } else {
            Exceptions.NoSceneController();
        }
    }
}

