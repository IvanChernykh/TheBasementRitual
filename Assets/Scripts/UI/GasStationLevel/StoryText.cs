using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utils;
using UnityEngine.Localization;

public class StoryText : MonoBehaviour {
    private readonly string[] texts = {
        "After work, you were about to head home when a homeless guy showed up and asked for a ride and to make a call. For some reason, you agreed.",
        "Apparently, he was the most popular homeless man on the planet - he was staying connected the whole time.",
        "By the time you dropped him off, you were out of gas, and he wore out your battery so you could not call automobile service.",
        "Now you're stuck in the middle of nowhere and need to find help."
    };

    private readonly string[] tuts = {
        "[W / A / S / D] - move",
        "[Left Shift] - sprint",
        "[Left CTRL] - crouch",
        "[E] - Interact"
    };

    private readonly LocalizedString[] storyTexts = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "AfterWork" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "MostPopularHomeless" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "OutOfGas" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Stuck" }
    };

    private readonly LocalizedString[] tutorialTexts = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Move" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Sprint" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Crouch" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Interact" }
    };

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

        yield return new WaitForSeconds(1f);

        foreach (var localizedString in storyTexts) {
            bool isTextReady = false;
            void OnTextChanged(string localizedText) {
                text.text = localizedText;
                isTextReady = true;
            }
            localizedString.StringChanged += OnTextChanged;
            localizedString.RefreshString();

            yield return new WaitUntil(() => isTextReady);
            localizedString.StringChanged -= OnTextChanged;

            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: true));
            yield return new WaitForSeconds(displayTime);
            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: false));
            yield return new WaitForSeconds(pauseBetweenTexts);
        }

        PlayerController.Instance.EnableCharacterController();
        yield return StartCoroutine(UI.FadeGraphic(background, fadeDuration, fadeIn: false));
        PlayerController.Instance.EnableCameraLook();

        string[] tutorialLocalizedTexts = new string[tutorialTexts.Length];
        for (int i = 0; i < tutorialTexts.Length; i++) {
            bool isTextReady = false;
            void OnTextChanged(string localizedText) {
                tutorialLocalizedTexts[i] = localizedText;
                isTextReady = true;
            }

            tutorialTexts[i].StringChanged += OnTextChanged;
            tutorialTexts[i].RefreshString();

            yield return new WaitUntil(() => isTextReady);
            tutorialTexts[i].StringChanged -= OnTextChanged;
        }

        TooltipUI.Instance.Show(tutorialLocalizedTexts, showTime: 3f);
        Destroy(gameObject);
    }
}