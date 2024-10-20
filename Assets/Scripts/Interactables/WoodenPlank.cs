using UnityEngine;

public class WoodenPlank : Interactable {
    [SerializeField] private ItemData hammer;
    [SerializeField] private BlockedDoor door;

    private void Start() {
        interactMessage = "Remove";
    }
    protected override void Interact() {
        if (PlayerInventory.Instance.HasItem(hammer)) {
            door.RemovePlank();
            Destroy(gameObject);
        } else {
            TooltipUI.Instance.Show("No items");
        }
    }
}
