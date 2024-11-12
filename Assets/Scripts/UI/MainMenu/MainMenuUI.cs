using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private GameObject continueGameBtn;
    [SerializeField] private TextMeshProUGUI creditButtonText;

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
    public void CreditsBtn() {
        creditButtonText.color = Color.white;
        CreditsPanel.Instance.Show();
    }
    public void ExitBtn() {
        SteamManager.Instance.Disconnect();
        Application.Quit();
    }
}
