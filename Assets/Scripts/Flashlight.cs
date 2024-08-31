using System;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    public static Flashlight Instance { get; private set; }
    [SerializeField] private GameObject flashlight;
    private bool isActive;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        InputManager.Instance.OnFlashlightToggleEvent += OnFlashLightToggle;
    }
    private void OnFlashLightToggle(object sender, EventArgs e) {
        if (!PlayerInventory.Instance.hasFlashlight) {
            return;
        }
        if (isActive) {
            UnEquip();
        } else {
            Equip();
        }
    }
    public void Equip() {
        flashlight.SetActive(true);
        isActive = true;
    }
    private void UnEquip() {
        flashlight.SetActive(false);
        isActive = false;
    }
}
