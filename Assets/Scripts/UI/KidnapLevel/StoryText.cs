using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour {

    private readonly string text1 = "After work, you were about to head home when a homeless guy showed up and asked for a ride and to make a call. For some reason, you agreed.";
    private readonly string text2 = "Apparently, he was the most popular homeless man on the planet — he was staying connected the whole time.";
    private readonly string text3 = "By the time you dropped him off, you were out of gas, and he wore out your battery so you could not call automobile service.";
    private readonly string text4 = "Now you're stuck in the middle of nowhere and need to find help.";

    private readonly string tut1 = "[W / A / S / D] - move";
    private readonly string tut2 = "[Left Shift] - sprint";
    private readonly string tut3 = "[Left CTRL] - crouch";
    private readonly string tut4 = "[E] - Interact";

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image background;

    [SerializeField] private float displayTime = 5f;
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float pauseBetweenTexts = 2f;

    private void Start() {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText() {
        PlayerController.Instance.DisableCharacterController();
        PlayerController.Instance.DisableCameraLook();

        string[] texts = { text1, text2, text3, text4 };
        string[] tuts = { tut1, tut2, tut3, tut4 };

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < texts.Length; i++) {
            text.text = texts[i];

            yield return StartCoroutine(FadeGraphic(text, fadeDuration, fadeIn: true));
            yield return new WaitForSeconds(displayTime);

            yield return StartCoroutine(FadeGraphic(text, fadeDuration, fadeIn: false));
            yield return new WaitForSeconds(pauseBetweenTexts);
        }
        PlayerController.Instance.EnableCharacterController();

        yield return StartCoroutine(FadeGraphic(background, fadeDuration, fadeIn: false));

        PlayerController.Instance.EnableCameraLook();

        yield return null;

        TooltipUI.Instance.Show(tuts, showTime: 3f);
        Destroy(gameObject);
    }
    private IEnumerator FadeGraphic(Graphic graphic, float duration, bool fadeIn) {
        float elapsedTime = 0f;
        Color color = graphic.color;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            graphic.color = color;
            yield return null;
        }

        color.a = endAlpha;
        graphic.color = color;
    }
}