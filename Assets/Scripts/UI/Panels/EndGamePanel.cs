using System.Collections;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public enum EndGameVariants {
    GiveUp,
    RunAway,
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
    private string[] runAwayEndGameText = { };
    private string[] banishDemonEndGameText = { };

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
            EndGameVariants.RunAway => runAwayEndGameText,
            EndGameVariants.BanishDemon => banishDemonEndGameText,
            _ => giveUpEndGameText,
        };
    }
}
