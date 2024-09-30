using System;
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

    public void StartNewGame() {
        LoadScene(GameScenes.BasementLevel);
    }

    public void LoadOfficeLevel() {
        LoadScene(GameScenes.OfficeLevel);
    }
    public void LoadGame(SaveFileName fileName) {
        // need to get scene from save file
        // need to save scene to save file
        SaveData saveData = SaveSystem.LoadSaveFile(fileName);
        // LoadScene(saveData.sceneData.scene, saveData)
    }

    private void LoadScene(GameScenes sceneToLoad, SaveData saveData) {
        StartCoroutine(LoadSceneAsync(sceneToLoad, saveData));
    }
    private void LoadScene(GameScenes sceneToLoad) {
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(GameScenes sceneToLoad, SaveData saveData = null) {
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

        if (saveData != null) {
            SetupGameData(saveData);
        }
    }

    public void SetupGameData(SaveData saveData) {
        // player data
        PlayerData playerData = saveData.playerData;

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
