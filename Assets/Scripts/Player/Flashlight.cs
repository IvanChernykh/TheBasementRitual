using System;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Utils;

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

    public float lifeTimeMax { get; private set; } = 120f;
    public float lifetime { get; private set; } = 120f;
    public float lightIntensity { get; private set; } = 1f;
    public float lightRange { get => lightSource.GetComponent<Light>().range; }

    public readonly float intensityMin = .6f;
    public readonly float intensityMax = 1f;


    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start() {
        Deactivate();
        InputManager.Instance.OnFlashlightToggleEvent += OnFlashLightToggle;
        InputManager.Instance.OnReloadBattery += OnRealoadBattery;
        InputManager.Instance.OnMouseScroll += OnFlashlightIntensityChanged;
    }
    private void Update() {
        HandleBatteryCharge();
    }
    private void OnRealoadBattery(object sender, EventArgs e) {
        if (!isActive) {
            return;
        }
        PlayerInventory playerInventory = PlayerInventory.Instance;
        if (playerInventory.batteries > 0) {
            animator.SetTrigger("Reload");
            playerInventory.RemoveBattery();
            StartCoroutine(ReloadBattery());
        } else {
            TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("NoBatteries"));
        }
    }
    private void OnFlashLightToggle(object sender, EventArgs e) {
        if (!PlayerInventory.Instance.hasFlashlight) {
            return;
        }
        if (isActive) {
            UnEquip();
        } else if (!PlayerController.Instance.isHiding) {
            Equip();
        }
    }
    public void Equip() {
        PlayerSounds.Instance.PlayFlashlightOnSound();
        flashlight.SetActive(true);
        animator.SetTrigger("Equipped");
        isActive = true;
        BatteryUI.Instance.Show();
    }
    public void UnequipImmediately() {
        Deactivate();
    }
    public void UnEquip() {
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
        BatteryUI.Instance.Hide();
    }
    private void OnFlashlightIntensityChanged(object sender, EventArgs e) {
        if (isActive &&
            lightSource.TryGetComponent(out Light light)) {
            float scrollValue = InputManager.Instance.GetMouseScrollValue();

            if (scrollValue != 0) {
                light.intensity = Mathf.Clamp(light.intensity + Mathf.Sign(scrollValue) * .1f, intensityMin, intensityMax);
                lightIntensity = light.intensity;
            }
        }
    }
}
