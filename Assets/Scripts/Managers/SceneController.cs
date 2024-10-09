using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static SceneController Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void MainMenu() {
        LoadScene(GameScenes.MainMenu);
    }
    public void StartNewGame() {
        LoadScene(GameScenes.GasStationLevel, saveOnLoad: true);
    }
    public void LoadNextLevel(GameScenes sceneToLoad) {
        SaveData saveData = new SaveData();
        LoadScene(sceneToLoad, saveData, isNextLevel: true, saveOnLoad: true);
    }
    public void LoadSavedGame(SaveFileName fileName) {
        SaveData saveData = SaveSystem.LoadSaveFile(fileName);
        LoadScene(saveData.sceneData.scene, saveData);
    }

    private void LoadScene(GameScenes sceneToLoad, SaveData saveData, bool isNextLevel = false, bool saveOnLoad = false) {
        StartCoroutine(LoadSceneAsync(sceneToLoad, saveData, isNextLevel: isNextLevel, saveOnLoad: saveOnLoad));
    }
    private void LoadScene(GameScenes sceneToLoad, bool saveOnLoad = false) {
        StartCoroutine(LoadSceneAsync(sceneToLoad, saveOnLoad: saveOnLoad));
    }

    private IEnumerator LoadSceneAsync(GameScenes sceneToLoad, SaveData saveData = null, bool isNextLevel = false, bool saveOnLoad = false) {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(GameScenes.LoadingScreen.ToString());
        yield return loadingOperation;

        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        sceneOperation.allowSceneActivation = false;

        while (sceneOperation.progress < 0.9f) {
            yield return null;
        }
        sceneOperation.allowSceneActivation = true;

        while (!sceneOperation.isDone) {
            yield return null;
        }

        // yield return null; // to make sure scene is ready

        if (saveData != null) {
            SetupGameData(saveData, isNextLevel);
        }
        if (saveOnLoad) {
            SaveSystem.SaveGame();
        }
    }

    public void SetupGameData(SaveData saveData, bool isNextLevel = false) {
        if (isNextLevel) {
            SetupPlayerData(saveData, setPosition: false);
        } else {
            SetupPlayerData(saveData);
            SetupSceneData(saveData);
        }

    }
    private void SetupPlayerData(SaveData saveData, bool setPosition = true) {
        PlayerData playerData = saveData.playerData;

        if (setPosition) {
            float posX = playerData.position[0];
            float posY = playerData.position[1];
            float posZ = playerData.position[2];

            float rotY = playerData.rotation[1];

            PlayerController.Instance.DisableCharacterController();
            PlayerController.Instance.transform.position = new Vector3(posX, posY, posZ);
            PlayerController.Instance.transform.rotation = Quaternion.Euler(0, rotY, 0);
            PlayerController.Instance.EnableCharacterController();
        }

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
    }
    private void SetupSceneData(SaveData saveData) {
        if (!SceneStateManager.Instance) {
            return;
        }
        SceneData sceneData = saveData.sceneData;

        // batteries
        foreach (int itemId in sceneData.batteriesCollected) {
            SceneStateManager.Instance.CollectBattery(itemId);
            var batteries = FindObjectsOfType<BatteryItem>();

            foreach (var battery in batteries) {
                if (battery.BatteryId == itemId) {
                    Destroy(battery.gameObject);
                    break;
                }
            }
        }
        // keys
        foreach (string itemId in sceneData.keysCollected) {
            SceneStateManager.Instance.CollectKey(itemId);
            var worldItems = FindObjectsOfType<WorldItem>();

            foreach (var item in worldItems) {
                if (item.ItemId == itemId) {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }
        // checkpoints
        foreach (string id in sceneData.checkpoints) {
            SceneStateManager.Instance.AddCheckpoint(id);
            var checkpoints = FindObjectsOfType<Checkpoint>();

            foreach (var item in checkpoints) {
                if (item.Id == id) {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }
        // doors
        foreach (DoorState door in sceneData.doors) {
            SceneStateManager.Instance.AddOrUpdateDoorState(door);
            var allDoors = FindObjectsOfType<DoorBase>();

            foreach (var item in allDoors) {
                if (item.Id == door.id) {
                    item.SetState(door);
                    break;
                }
            }
        }
    }
}
