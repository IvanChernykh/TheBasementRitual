using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public static PlayerInventory Instance { get; private set; }
    public List<ItemData> items { get; private set; } = new List<ItemData>();
    public bool hasFlashlight { get; private set; }

    private void Awake() {
        Instance = this;
    }
    public void SetHasFlashlight(bool has) {
        hasFlashlight = has;
    }
    public void AddItem(ItemData item) {
        items.Add(item);
    }
    public void RemoveItem(ItemData item) {
        items.Remove(item);
    }
}
