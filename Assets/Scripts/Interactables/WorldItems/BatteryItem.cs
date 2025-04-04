using Assets.Scripts.Utils;
using UnityEngine;

public class BatteryItem : Interactable {
    [SerializeField] private int id;

    public int BatteryId { get => id; }

    private void Start() {
        interactMessage = "Take";
    }
    protected override void Interact() {
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries < playerInventory.batteriesMax) {
            playerInventory.AddBattery();
            SceneStateManager.Instance.CollectBattery(id);
            GameUI.Tooltip.Show(LocalizationHelper.LocalizeTooltip("BatteryPickUp"));
            Destroy(gameObject);
        } else {
            GameUI.Tooltip.Show(LocalizationHelper.LocalizeTooltip("BatteriesMax"));
        }
    }
}
