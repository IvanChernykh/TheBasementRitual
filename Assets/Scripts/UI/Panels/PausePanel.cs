using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public class PausePanel : MonoBehaviour {

    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI resumeText;
    [SerializeField] private TextMeshProUGUI lastSaveText;
    [SerializeField] private TextMeshProUGUI optionsText;
    [SerializeField] private TextMeshProUGUI mainMenuText;

    private void Start() {
        Hide();
    }
    public void Show() {
        panel.SetActive(true);
    }
    public void Hide() {
        resumeText.color = Color.white;
        lastSaveText.color = Color.white;
        optionsText.color = Color.white;
        mainMenuText.color = Color.white;

        panel.SetActive(false);
    }
    // buttons
    public void ResumeGame() {
        GameStateManager.Instance.ExitPausedState();
        GameStateManager.Instance.EnterInGameState();
    }
    public void LoadLastGame() {
        if (SceneController.Instance != null) {
            SceneController.Instance.LoadSavedGame(SaveFileName.DefaultSave);
        } else {
            Exceptions.NoSceneController();
        }
    }
    public void Options() {
        OptionsPanel.Instance.Show();
    }
    public void MainMenu() {
        if (SceneController.Instance != null) {
            GameStateManager.Instance.EnterMainMenuState();
            SceneController.Instance.MainMenu();
        } else {
            Exceptions.NoSceneController();
        }
    }
}
