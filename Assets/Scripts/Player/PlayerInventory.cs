using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInventory : Singleton<PlayerInventory> {
    public List<ItemData> items { get; private set; } = new List<ItemData>();

    public bool hasFlashlight { get; private set; }
    public int batteries { get; private set; } = 0;
    public readonly int batteriesMax = 10;

    private void Awake() {
        InitializeSingleton();
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
    public bool HasItem(ItemData item) {
        return items.Contains(item);
    }
    public void AddBattery(int count = 1) {
        batteries += count;
    }
    public void RemoveBattery() {
        batteries--;
    }
}
