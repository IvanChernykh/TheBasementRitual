using UnityEngine;

public class LightFlickering : MonoBehaviour {
    [SerializeField] private bool enableFlickering = true;
    [SerializeField] private Light lightSource;
    [SerializeField] private float timerMin = .1f;
    [SerializeField] private float timerMax = .9f;
    [SerializeField] private Material lampMat;
    private bool emit = true;
    private float timer;

    private void Start() {
        timer = Random.Range(timerMin, timerMax);
    }
    private void Update() {
        if (enableFlickering) {
            Flicker();
        }
    }
    private void Flicker() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0) {
            emit = !emit;
            lightSource.enabled = !lightSource.enabled;
            if (emit) {
                lampMat.EnableKeyword("_EMISSION");
            } else {
                lampMat.DisableKeyword("_EMISSION");
            }
            timer = Random.Range(timerMin, timerMax);
        }
    }
}
