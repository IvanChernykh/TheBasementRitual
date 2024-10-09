using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    public void StartNewGameBtn() {
        SceneController.Instance.StartNewGame();
    }
    // todo: disable if there is no save files
    public void ContinueGameBtn() {
        SceneController.Instance.LoadSavedGame(SaveFileName.DefaultSave);
    }
    public void OptionsBtn() {

    }
    public void ExitBtn() {
        Application.Quit();
    }
}
