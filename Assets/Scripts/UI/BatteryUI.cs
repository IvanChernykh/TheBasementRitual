using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour {
    public static BatteryUI Instance { get; private set; }
    [SerializeField] private Image background;
    [SerializeField] private Image chargeBar;
    [SerializeField] private TextMeshProUGUI counter;
    private bool isShown;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        Hide();
    }
    private void Update() {
        if (Flashlight.Instance.isActive && !isShown) {
            Show();
        }
        if (!Flashlight.Instance.isActive && isShown) {
            Hide();
        }
        HandleCharge();
        HandleBatteryCounter();
    }
    public void Hide() {
        isShown = false;
        background.gameObject.SetActive(false);
        chargeBar.gameObject.SetActive(false);
        counter.gameObject.SetActive(false);
    }
    public void Show() {
        isShown = true;
        background.gameObject.SetActive(true);
        chargeBar.gameObject.SetActive(true);
        counter.gameObject.SetActive(true);
    }
    public void HandleCharge() {
        chargeBar.fillAmount = Flashlight.Instance.lifetime / Flashlight.Instance.lifeTimeMax;
    }
    public void HandleBatteryCounter() {
        counter.text = $"{PlayerInventory.Instance.batteries.Count} / {PlayerInventory.Instance.batteriesMax}";
    }
}
