using System;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI inventoryText;

    private bool isActive = false;

    private void Start() {
        InputManager.Instance.OnInventoryEvent += OnInventoryEvent;
        InputManager.Instance.OnInventoryCanceledEvent += OnInventoryCanceledEvent;
    }

    private void Update() {
        if (isActive && !PlayerController.Instance.IsControllerEnabled()) {
            Hide();
        }
    }

    private void OnInventoryEvent(object sender, EventArgs e) {
        isActive = true;
        Show();
    }
    private void OnInventoryCanceledEvent(object sender, EventArgs e) {
        isActive = false;
        Hide();
    }

    private void Show() {
        var items = PlayerInventory.Instance.items.FindAll(item => !item.itemName.Contains("TurnOnLights"));
        string itemsText = "";

        if (items.Count == 0) {
            itemsText = LocalizationHelper.LocalizeTooltip("InventoryIsEmpty");
        } else {
            foreach (ItemData item in items) {
                itemsText += LocalizationHelper.LocalizeTooltip(item.itemName) + "\n";
            }
        }

        inventoryText.text = itemsText;
        container.SetActive(true);
    }
    private void Hide() {
        container.SetActive(false);
    }
}
