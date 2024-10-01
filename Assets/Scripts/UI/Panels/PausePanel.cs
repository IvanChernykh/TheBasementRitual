using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour {
    public static PausePanel Instance { get; private set; }
    [SerializeField] private GameObject panel;
    [Header("Buttons")]
    [SerializeField] private Button loadGameButton;
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
    // temp
    public void LoadLastGame() {
        if (SceneController.Instance != null) {
            SceneController.Instance.LoadSavedGame(SaveFileName.DefaultSave);
        } else {
            Debug.LogWarning("There is no scene controller");
        }
    }
}
