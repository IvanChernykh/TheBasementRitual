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
            TooltipUI.Instance.Show("Seems like it leads back to the basement. I'd better find another way.");
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
