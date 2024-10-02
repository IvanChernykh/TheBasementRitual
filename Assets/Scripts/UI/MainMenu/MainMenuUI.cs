using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    public void StartNewGameBtn() {
        SceneController.Instance.StartNewGame();
    }
    public void ContinueGameBtn() {
        SceneController.Instance.LoadSavedGame(SaveFileName.DefaultSave);
    }
    public void ExitBtn() {
        Application.Quit();
    }
}
