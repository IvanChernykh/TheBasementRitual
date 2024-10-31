using UnityEngine;

public class GlowingObjects : MonoBehaviour {
    [SerializeField] private Material[] materials;
    [SerializeField] private Color emissionColor = Color.white;
    [SerializeField] private float minEmission = 0f;
    [SerializeField] private float maxEmission = .05f;
    [SerializeField] private float blinkSpeed = 2f;

    private void Update() {
        HandleEmission();
    }
    private void HandleEmission() {
        float emissionValue = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        emissionValue = Mathf.Lerp(minEmission, maxEmission, emissionValue);

        Color finalColor = emissionColor * Mathf.LinearToGammaSpace(emissionValue);
        foreach (Material mat in materials) {
            mat.EnableKeyword("_EMISSION");
            mat.SetVector("_EmissionColor", finalColor);
        }
    }
}
