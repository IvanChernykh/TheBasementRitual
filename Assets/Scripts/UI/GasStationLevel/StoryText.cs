using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utils;
using UnityEngine.Localization;
using System.Linq;

public class StoryText : MonoBehaviour {
    private readonly LocalizedString[] storyTexts = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Intro1" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Intro2" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Intro3" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "Intro4" }
    };

    private readonly LocalizedString[] tutorialTexts = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "TutMove" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "TutSprint" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "TutCrouch" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "TutInteract" }
    };

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI nextBtnText;
    [SerializeField] private Image background;

    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float pauseBetweenTexts = 1f;

    private void Start() {
        InputManager.Instance.SetCanPause(false);
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText() {
        PlayerController.Instance.DisableCharacterController();
        PlayerController.Instance.DisableCameraLook();

        yield return new WaitForSeconds(1f);

        StartCoroutine(UI.FadeGraphic(nextBtnText, fadeDuration, fadeIn: true));

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
            yield return WaitForPlayerInput();

            if (localizedString == storyTexts.Last()) {
                StartCoroutine(UI.FadeGraphic(nextBtnText, fadeDuration, fadeIn: false));
            }

            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: false));
            yield return new WaitForSeconds(pauseBetweenTexts);
        }

        PlayerController.Instance.EnableCharacterController();
        yield return StartCoroutine(UI.FadeGraphic(background, fadeDuration, fadeIn: false));
        PlayerController.Instance.EnableCameraLook();

        InputManager.Instance.SetCanPause(true);

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

    private IEnumerator WaitForPlayerInput() {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }
}