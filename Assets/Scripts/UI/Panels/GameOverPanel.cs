using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {
    public static GameOverPanel Instance { get; private set; }
    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        loadGameButton.onClick.AddListener(() => {
            LoadLastGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            MainMenu();
        });
    }
    private void Start() {
        Hide();
    }
    public void Show() {
        panel.SetActive(true);
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
            SceneController.Instance.MainMenu();
        } else {
            Exceptions.NoSceneController();
        }
    }
}
