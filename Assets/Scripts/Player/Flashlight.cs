using System;
using System.Collections;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    public static Flashlight Instance { get; private set; }
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject lightSource;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private readonly float unequipAnimationDuration = .1f;
    private readonly float reloadAnimationDelay = .1f;
    private readonly float reloadAnimationDuration = .8f;
    public bool isActive { get; private set; }

    public float lifeTimeMax { get; private set; } = 100f;
    public float lifetime { get; private set; } = 100f;


    private void Awake() {
        Instance = this;
    }
    private void Start() {
        Deactivate();
        InputManager.Instance.OnFlashlightToggleEvent += OnFlashLightToggle;
        InputManager.Instance.OnReloadBattery += OnRealoadBattery;
    }
    private void Update() {
        HandleBatteryCharge();
    }
    private void OnRealoadBattery(object sender, EventArgs e) {
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries.Count > 0) {
            animator.SetTrigger("Reload");
            playerInventory.RemoveBattery();
            StartCoroutine(ReloadBattery());
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
        animator.SetTrigger("Equipped");
        isActive = true;
    }
    private void UnEquip() {
        animator.SetTrigger("Unequip");
        StartCoroutine(DeactivateAfterDelay(unequipAnimationDuration));
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
    private IEnumerator DeactivateAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }
    private IEnumerator ReloadBattery() {
        yield return new WaitForSeconds(reloadAnimationDelay);
        lightSource.SetActive(false);
        yield return new WaitForSeconds(reloadAnimationDuration);
        lifetime = lifeTimeMax;
        lightSource.SetActive(true);

    }
    private void Deactivate() {
        flashlight.SetActive(false);
        isActive = false;
    }
}
