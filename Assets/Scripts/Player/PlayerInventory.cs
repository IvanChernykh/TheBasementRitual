using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;

public class PlayerInventory : MonoBehaviour {
    public static PlayerInventory Instance { get; private set; }
    public List<ItemData> items { get; private set; } = new List<ItemData>();
    public List<ItemData> batteries { get; private set; } = new List<ItemData>();
    public bool hasFlashlight { get; private set; }
    public int batteriesMax { get; private set; } = 10;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
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
    public void AddBattery(ItemData battery) {
        batteries.Add(battery);
    }
    public void RemoveBattery() {
        batteries.RemoveAt(0);
    }
}
