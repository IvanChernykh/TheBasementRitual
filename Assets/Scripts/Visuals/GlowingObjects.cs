using UnityEngine;

public class GlowingObjects : MonoBehaviour {
    public Material[] materials;
    public Color emissionColor = Color.white;
    public float minEmission = 0f;
    public float maxEmission = .05f;
    public float blinkSpeed = 2f;

    void Start() {
        foreach (Material mat in materials) {
            mat.EnableKeyword("_EMISSION");
        }
    }

    private void Update() {
        HandleEmission();
    }
    private void HandleEmission() {
        float emissionValue = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        emissionValue = Mathf.Lerp(minEmission, maxEmission, emissionValue);

        Color finalColor = emissionColor * Mathf.LinearToGammaSpace(emissionValue);
        foreach (Material mat in materials) {
            mat.SetColor("_EmissionColor", finalColor);
        }
    }
}
