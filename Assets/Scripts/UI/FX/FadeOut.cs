using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUI : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private float fadeTime;
    [SerializeField] private float delay;
    [SerializeField] private bool destroySelf;

    private void Start() {
        StartCoroutine(FadeOut(delay));
    }

    private IEnumerator FadeOut(float effectDelay) {
        if (effectDelay > 0) {
            yield return new WaitForSeconds(effectDelay);
        }
        yield return UI.FadeGraphic(image, fadeTime, fadeIn: false);

        if (destroySelf) {
            Destroy(gameObject);
        }
    }
}
