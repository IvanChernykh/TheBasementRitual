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
        // player data
        PlayerData playerData = data.playerData;

        float posX = playerData.position[0];
        float posY = playerData.position[1];
        float posZ = playerData.position[2];

        float rotY = playerData.rotation[1];

        PlayerController.Instance.DisableCharacterController();
        PlayerController.Instance.transform.position = new Vector3(posX, posY, posZ);
        PlayerController.Instance.transform.rotation = Quaternion.Euler(0, rotY, 0);
        PlayerController.Instance.EnableCharacterController();

        PlayerInventory.Instance.SetHasFlashlight(playerData.hasFlashlight);
        PlayerInventory.Instance.AddBattery(playerData.batteryCount);

        foreach (string itemName in playerData.items) {
            if (AllKeys.Instance.HasItem(itemName)) {
                ItemData item = AllKeys.Instance.GetItem(itemName);

                if (item != null) {
                    PlayerInventory.Instance.AddItem(item);
                }
            }
        }

        // SceneData sceneData = data.sceneData;
        // foreach (int item in sceneData.batteriesCollected) {
        //     SceneStateManager.Instance.CollectBattery(item);
        // }
        // foreach (string item in sceneData.keysCollected) {
        //     SceneStateManager.Instance.CollectKey(item);
        // }
    }
}
