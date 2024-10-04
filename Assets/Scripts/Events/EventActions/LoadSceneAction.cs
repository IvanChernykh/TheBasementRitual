using System.Collections;
using UnityEngine;

public class LoadSceneAction : EventAction {
    [SerializeField] private GameScenes sceneToLoad;
    [SerializeField] private float delay = 0f;

    public override void ExecuteEvent() {
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
