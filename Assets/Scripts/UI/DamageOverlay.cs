using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour {
    public static DamageOverlay Instance { get; private set; }
    [SerializeField] private Image overlayImage;
    private float defaultImageAlpha;
    private float timer;
    private float timerMax;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        defaultImageAlpha = overlayImage.color.a;
        overlayImage.gameObject.SetActive(false);
    }

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;

            SetOverlayAlpha(Mathf.Clamp01(timer / timerMax) * defaultImageAlpha);

            if (timer <= 0) {
                overlayImage.gameObject.SetActive(false);
                SetOverlayAlpha(defaultImageAlpha);
            }
        }
    }
    public void Show(float timerMaxValue, float restoreDelay) {
        SetOverlayAlpha(defaultImageAlpha);
        StopAllCoroutines();
        StartCoroutine(SetHidingTimer(timerMaxValue, restoreDelay));
        if (!overlayImage.gameObject.activeSelf) {
            overlayImage.gameObject.SetActive(true);
        }
    }

    private void SetOverlayAlpha(float alpha) {
        Color newColor = overlayImage.color;
        newColor.a = alpha;
        overlayImage.color = newColor;
    }
    private IEnumerator SetHidingTimer(float timerMaxValue, float delay) {
        yield return new WaitForSeconds(delay);
        timerMax = timerMaxValue;
        timer = timerMax;
    }
}
