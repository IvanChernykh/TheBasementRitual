using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private GameObject continueGameBtn;

    private void Start() {
        if (SaveSystem.SaveFileExists(SaveFileName.DefaultSave)) {
            continueGameBtn.SetActive(true);
        } else {
            continueGameBtn.SetActive(false);
        }
    }
    public void StartNewGameBtn() {
        SceneController.Instance.StartNewGame();
    }
    public void ContinueGameBtn() {
        SceneController.Instance.LoadSavedGame(SaveFileName.DefaultSave);
    }
    public void OptionsBtn() {

    }
    public void ExitBtn() {
        Application.Quit();
    }
}
