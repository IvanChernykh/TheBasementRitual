using System.Collections;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public enum EndGameVariants {
    GiveUp,
    Escape,
    BanishDemon
}

public class EndGamePanel : MonoBehaviour {
    public static EndGamePanel Instance { get; private set; }

    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI text;

    private readonly LocalizedString[] giveUpEndGameText = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndGiveUp1" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndGiveUp2" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndGiveUp3" }
    };

    private readonly LocalizedString[] escapeEndGameText = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndEscape1" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndEscape2" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndEscape3" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndEscape4" }
    };

    private readonly LocalizedString[] banishDemonEndGameText = {
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndBanish1" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndBanish2" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndBanish3" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndBanish4" },
        new LocalizedString { TableReference = LocalizationTables.IntroAndEndings, TableEntryReference = "EndBanish5" }
    };

    private readonly float displayTime = 5f;
    private readonly float fadeDuration = 0.8f;
    private readonly float pauseBetweenTexts = 1f;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Show(EndGameVariants currentEndGame) {
        InputManager.Instance.SetCanPause(false);
        container.SetActive(true);
        StartCoroutine(EndGameRoutine(currentEndGame));
    }
    public void Hide() {
        container.SetActive(false);
    }

    private IEnumerator EndGameRoutine(EndGameVariants currentEndGame) {
        LocalizedString[] endGameTexts = GetEndGameText(currentEndGame);
        string[] localizedTexts = new string[endGameTexts.Length];

        yield return new WaitForSecondsRealtime(2.5f);

        for (int i = 0; i < endGameTexts.Length; i++) {
            bool isTextReady = false;

            void OnTextChanged(string localizedText) {
                localizedTexts[i] = localizedText;
                isTextReady = true;
            }

            endGameTexts[i].StringChanged += OnTextChanged;
            endGameTexts[i].RefreshString();

            yield return new WaitUntil(() => isTextReady);
            endGameTexts[i].StringChanged -= OnTextChanged;

            text.text = localizedTexts[i];
            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: true));
            yield return new WaitForSecondsRealtime(displayTime);

            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: false));
            yield return new WaitForSecondsRealtime(pauseBetweenTexts);
        }

        yield return new WaitForSecondsRealtime(1f);
        InputManager.Instance.SetCanPause(true);

        if (SceneController.Instance != null) {
            GameStateManager.Instance.EnterMainMenuState();
            SceneController.Instance.MainMenu();
        } else {
            Exceptions.NoSceneController();
        }
    }

    private LocalizedString[] GetEndGameText(EndGameVariants currentEndGame) {
        return currentEndGame switch {
            EndGameVariants.GiveUp => giveUpEndGameText,
            EndGameVariants.Escape => escapeEndGameText,
            EndGameVariants.BanishDemon => banishDemonEndGameText,
            _ => giveUpEndGameText,
        };
    }
}
