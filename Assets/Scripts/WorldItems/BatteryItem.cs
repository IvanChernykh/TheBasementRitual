using UnityEngine;

public class BatteryItem : Interactable {
    [SerializeField] private ItemData itemData;
    private void Start() {
        interactMessage = "Take";
    }
    public override void Interact() {
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries.Count < playerInventory.batteriesMax) {
            playerInventory.AddBattery(itemData);
            Destroy(gameObject);
            TooltipUI.Instance.Show("Picked up a battery");
        } else {
            TooltipUI.Instance.Show("Can't carry more batteries");
        }
    }
}
