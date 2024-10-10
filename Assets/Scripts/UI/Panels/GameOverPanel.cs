using Assets.Scripts.Utils;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {
    public static GameOverPanel Instance { get; private set; }
    [SerializeField] private GameObject panel;

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
