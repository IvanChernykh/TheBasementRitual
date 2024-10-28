using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;

[Serializable]
public class PlayerInventory : MonoBehaviour {
    public static PlayerInventory Instance { get; private set; }
    public List<ItemData> items { get; private set; } = new List<ItemData>();

    public bool hasFlashlight { get; private set; }
    public int batteries { get; private set; } = 0;
    public readonly int batteriesMax = 10;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            foreach (ItemData item in items) {
                Debug.Log(item);
            }
        }
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
