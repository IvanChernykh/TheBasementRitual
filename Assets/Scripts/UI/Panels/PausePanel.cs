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
        SaveData data = SaveSystem.LoadGame(SaveFileName.DefaultSave);
        PlayerData playerData = data.playerData;
        SceneData sceneData = data.sceneData;

        float posX = playerData.position[0];
        float posY = playerData.position[1];
        float posZ = playerData.position[2];

        float rotY = playerData.rotation[1];

        PlayerController.Instance.DisableCharacterController();
        PlayerController.Instance.transform.position = new Vector3(posX, posY, posZ);
        PlayerController.Instance.transform.rotation = Quaternion.Euler(0, rotY, 0);
        PlayerController.Instance.EnableCharacterController();

        PlayerInventory.Instance.SetHasFlashlight(playerData.hasFlashlight);

        foreach (int item in sceneData.batteriesCollected) {
            SceneStateManager.Instance.CollectBattery(item);
        }
        foreach (string item in sceneData.keysCollected) {
            SceneStateManager.Instance.CollectKey(item);
        }
    }
}
