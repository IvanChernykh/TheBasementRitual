using System;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    public static Flashlight Instance { get; private set; }
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject lightSource;
    private bool isActive;

    private float lifeTimeMax = 100f;
    private float lifetime = 100f;


    private void Awake() {
        Instance = this;
    }
    private void Start() {
        UnEquip();
        InputManager.Instance.OnFlashlightToggleEvent += OnFlashLightToggle;
        InputManager.Instance.OnReloadBattery += OnRealoadBattery;
    }
    private void Update() {
        HandleBatteryCharge();
    }
    private void OnRealoadBattery(object sender, EventArgs e) {
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries.Count > 0) {
            lifetime = lifeTimeMax;
            playerInventory.RemoveBattery();
            lightSource.SetActive(true);
        } else {
            TooltipUI.Instance.Show("No batteries");
        }
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
    private void HandleBatteryCharge() {
        if (!isActive) {
            return;
        }
        if (lifetime > 0) {
            lifetime -= Time.deltaTime;
        } else {
            lifetime = 0;
            lightSource.SetActive(false);
        }
    }
}
