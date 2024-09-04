using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour {
    public static TooltipUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI tooltip;
    [SerializeField] private float timerMax = 2f;
    private float timer;
    private bool show;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        tooltip.text = "";
        tooltip.gameObject.SetActive(false);
    }

    void Update() {
        if (show) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                timer = 0;
                show = false;
                Hide();
            }
        }
    }
    public void Show(string text) {
        show = true;
        timer = timerMax;

        tooltip.gameObject.SetActive(true);
        tooltip.text = text;
    }
    private void Hide() {
        tooltip.text = "";
        tooltip.gameObject.SetActive(false);
    }
}
