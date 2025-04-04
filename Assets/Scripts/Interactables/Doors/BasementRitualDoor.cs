using Assets.Scripts.Utils;
using UnityEngine;

public class BasementRitualDoor : Interactable {
    [Header("Door Settings")]
    [SerializeField] private ItemData[] neededItems;
    private void Start() {
        interactMessage = "Open";
    }
    protected override void Interact() {
        if (checkNeededItems()) {
            SceneController.Instance.LoadNextLevel(GameScenes.BasementRitualLevel);
        } else {
            GameUI.Tooltip.Show(LocalizationHelper.LocalizeTooltip("BasementDoorWayBack"));
        }
    }

    private bool checkNeededItems() {
        bool hasAllItems = true;

        foreach (ItemData item in neededItems) {
            if (!PlayerInventory.Instance.HasItem(item)) {
                hasAllItems = false;
                break;
            }
        }

        return hasAllItems;
    }
}
