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
        // PlayerData data = SaveSystem.LoadGame(SaveFileName.DefaultSave).playerData;
        // float posX = data.playerPosition[0];
        // float posY = data.playerPosition[1];
        // float posZ = data.playerPosition[2];

        // float rotY = data.playerRotation[1];

        // PlayerController.Instance.DisableCharacterController();
        // PlayerController.Instance.transform.position = new Vector3(posX, posY, posZ);
        // PlayerController.Instance.transform.rotation = Quaternion.Euler(0, rotY, 0);
        // PlayerController.Instance.EnableCharacterController();
    }
}
