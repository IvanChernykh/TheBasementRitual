using System.Collections;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public enum EndGameVariants {
    GiveUp,
    Escape,
    BanishDemon
}

public class EndGamePanel : MonoBehaviour {
    public static EndGamePanel Instance { get; private set; }

    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI text;

    private string[] giveUpEndGameText = {
        "Instead of trying to escape, you decided to do nothing and give up.",
        "Eventually, the demon emerged from the basement and killed everyone in the town.",
        "It took a long time before people figured out how to kill it, but far too many had already died."
        };
    private string[] escapeEndGameText = {
        "You successfully escaped from the basement, but the demon broke out along with you.",
        "Though you saved your life, you condemned others to death.",
        "The demon managed to kill many people before they figured out how to defeat him.",
        "Thank you for playing"
        };
    private string[] banishDemonEndGameText = {
        "You successfully banished the demon and escaped from the basement.",
        "You told the police everything that happened there, but they didn't believe the part about the demon.",
        "You were sent to an asylum for six months to recover your mental health.",
        "After that, you left the town forever.",
        "Thank you for playing"
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
        container.SetActive(true);
        StartCoroutine(EndGameRoutine(currentEndGame));
    }
    public void Hide() {
        container.SetActive(false);
    }

    private IEnumerator EndGameRoutine(EndGameVariants currentEndGame) {
        string[] endGameText = GetEndGameText(currentEndGame);

        yield return new WaitForSecondsRealtime(2.5f);

        for (int i = 0; i < endGameText.Length; i++) {
            text.text = endGameText[i];
            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: true));
            yield return new WaitForSecondsRealtime(displayTime);

            yield return StartCoroutine(UI.FadeGraphic(text, fadeDuration, fadeIn: false));
            yield return new WaitForSecondsRealtime(pauseBetweenTexts);
        }

        yield return new WaitForSecondsRealtime(1f);

        if (SceneController.Instance != null) {
            GameStateManager.Instance.EnterMainMenuState();
            SceneController.Instance.MainMenu();
        } else {
            Exceptions.NoSceneController();
        }
    }

    private string[] GetEndGameText(EndGameVariants currentEndGame) {
        return currentEndGame switch {
            EndGameVariants.GiveUp => giveUpEndGameText,
            EndGameVariants.Escape => escapeEndGameText,
            EndGameVariants.BanishDemon => banishDemonEndGameText,
            _ => giveUpEndGameText,
        };
    }
}
