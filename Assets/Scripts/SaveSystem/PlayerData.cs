using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData {
    // player
    public float[] position;
    public float[] rotation;

    // inventory
    public bool hasFlashlight;
    public int batteryCount;
    public string[] items;

    // flashlight
    public bool flashlightActive;
    public float flashlightLifetime;

    public PlayerData() {
        PlayerController player = PlayerController.Instance;
        PlayerInventory inventory = PlayerInventory.Instance;
        Flashlight flashlight = Flashlight.Instance;

        Vector3 pos = player.transform.position;
        Vector3 angles = player.transform.rotation.eulerAngles;

        position = new float[] { pos.x, pos.y, pos.z };
        rotation = new float[] { angles.x, angles.y, angles.z };

        hasFlashlight = inventory.hasFlashlight;
        batteryCount = inventory.batteries;
        items = inventory.items.Select(item => item.itemName).ToArray();

        flashlightActive = flashlight.isActive;
        flashlightLifetime = flashlight.lifetime;
    }
}
