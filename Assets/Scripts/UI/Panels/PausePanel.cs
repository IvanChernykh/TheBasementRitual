using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public class PausePanel : MonoBehaviour {
    public static PausePanel Instance { get; private set; }
    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI resumeText;
    [SerializeField] private TextMeshProUGUI optionsText;
    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start() {
        Hide();
    }
    public void Show() {
        panel.SetActive(true);
    }
    public void Hide() {
        resumeText.color = Color.white;
        optionsText.color = Color.white;
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
