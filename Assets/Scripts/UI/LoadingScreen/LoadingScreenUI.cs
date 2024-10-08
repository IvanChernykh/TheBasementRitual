using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float blinkSpeed = 1.5f;
    private readonly float minAlpha = .3f;
    private readonly float maxAlpha = 1f;

    private void Update() {
        HandleEmission();
    }

    private void HandleEmission() {
        float alphaValue = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        alphaValue = Mathf.Lerp(minAlpha, maxAlpha, alphaValue);

        Color currentColor = text.color;
        currentColor.a = alphaValue;

        text.color = currentColor;
    }
}
