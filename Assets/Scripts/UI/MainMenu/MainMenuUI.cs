using Assets.Scripts.Utils;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private GameObject continueGameBtn;

    private void Start() {
        UI.ShowCursor();
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
        OptionsPanel.Instance.Show();
    }
    public void ExitBtn() {
        Application.Quit();
    }
}
