using System.Collections;
using UnityEngine;

public class LoadSceneAction : EventAction {
    [Header("Action Settings")]
    [SerializeField] private GameScenes sceneToLoad;
    [SerializeField] private float delay = 0f;

    public override void ExecuteAction() {
        if (delay > 0) {
            StartCoroutine(ShowWithDelay());
        } else {
            LoadScene();
        }
    }
    private void LoadScene() {
        SceneController.Instance.LoadNextLevel(sceneToLoad);
    }

    private IEnumerator ShowWithDelay() {
        yield return new WaitForSeconds(delay);
        LoadScene();
    }
}
