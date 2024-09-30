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

    private void LoadScene(GameScenes sceneToLoad) {
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(GameScenes sceneToLoad) {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(GameScenes.LoadingScreen.ToString());
        yield return loadingOperation;

        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        sceneOperation.allowSceneActivation = false;

        while (!sceneOperation.isDone) {
            if (sceneOperation.progress >= 0.9f) {
                sceneOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    // public void SetupGameData(SaveData saveData) {
    //     PlayerData playerData = saveData.playerData;
    // }
}
