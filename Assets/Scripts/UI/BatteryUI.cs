using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour {

    [SerializeField] private Image background;
    [SerializeField] private Image chargeBar;
    [SerializeField] private TextMeshProUGUI counter;
    private bool isShown;
    private Color whiteColor = new Color(255, 255, 255, .58f);
    private Color redColor = new Color(255, 0, 0, .58f);
    private Color currentColor;

    private void Start() {
        Hide();
        counter.color = whiteColor;
        chargeBar.color = whiteColor;
        background.color = whiteColor;
        currentColor = whiteColor;
    }
    private void Update() {
        if (isShown) {
            HandleCharge();
            HandleBatteryCounter();
        }
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
        float chargePercent = Flashlight.Instance.lifetime / Flashlight.Instance.lifeTimeMax;
        chargeBar.fillAmount = chargePercent;

        if (chargePercent < .2f) {
            if (currentColor == whiteColor) {
                chargeBar.color = redColor;
                background.color = redColor;
                currentColor = redColor;
            }

        } else {
            if (currentColor == redColor) {
                chargeBar.color = whiteColor;
                background.color = whiteColor;
                currentColor = whiteColor;
            }
        }
    }
    public void HandleBatteryCounter() {
        counter.text = $"{PlayerInventory.Instance.batteries} / {PlayerInventory.Instance.batteriesMax}";
    }
}
