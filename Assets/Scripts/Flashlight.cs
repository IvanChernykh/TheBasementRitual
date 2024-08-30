using System;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    [SerializeField] private GameObject lightSource;
    private bool isActive = false;

    private void Start() {
        lightSource.SetActive(isActive);
        InputManager.Instance.OnFlashlightToggleEvent += OnFlashLightToggle;
    }
    private void OnFlashLightToggle(object sender, EventArgs e) {
        isActive = !isActive;
        lightSource.SetActive(isActive);
    }
}
