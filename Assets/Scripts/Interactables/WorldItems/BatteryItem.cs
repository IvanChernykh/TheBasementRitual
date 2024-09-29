using UnityEngine;

public class BatteryItem : Interactable {
    [SerializeField] private ItemData itemData;
    [SerializeField] private int id;

    private void Start() {
        interactMessage = "Take";

        if (SceneStateManager.Instance.batteriesCollected.Exists(item => item == id)) {
            Destroy(gameObject);
        }
    }

    private void Update() {
        // if (SceneStateManager.Instance.batteriesCollected.Exists(item => item == id)) {
        //     Destroy(gameObject);
        // }
    }
    protected override void Interact() {
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries.Count < playerInventory.batteriesMax) {
            playerInventory.AddBattery(itemData);
            SceneStateManager.Instance.CollectBattery(id);
            TooltipUI.Instance.Show("Picked up a battery");
            Destroy(gameObject);
        } else {
            TooltipUI.Instance.Show("Can't carry more batteries");
        }
    }
}
