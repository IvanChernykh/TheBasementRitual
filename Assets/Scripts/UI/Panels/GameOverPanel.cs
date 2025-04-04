using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject blackScreen;

    private void Start() {
        Hide();
    }
    public void Show() {
        blackScreen.SetActive(true);
        StartCoroutine(ShowPanelRoutine());
    }
    public void Hide() {
        panel.SetActive(false);
    }

    // buttons
    public void LoadLastGame() {
        if (SceneController.Instance != null) {
            SceneController.Instance.LoadSavedGame(SaveFileName.DefaultSave);
        } else {
            Exceptions.NoSceneController();
        }
    }
    public void MainMenu() {
        if (SceneController.Instance != null) {
            GameStateManager.Instance.EnterMainMenuState();
            SceneController.Instance.MainMenu();
        } else {
            Exceptions.NoSceneController();
        }
    }
    // enumerator
    private IEnumerator ShowPanelRoutine() {
        yield return new WaitForSecondsRealtime(2);
        UI.ShowCursor();
        blackScreen.SetActive(false);
        panel.SetActive(true);
    }
}
