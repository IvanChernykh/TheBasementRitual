using UnityEngine;
using Assets.Scripts.Utils;

public class GameStateManager : Singleton<GameStateManager> {

    private enum GameState {
        InGame,
        ReadingNote,
        Paused,
        GameOver,
        MainMenu,
        EndGame
    }
    private GameState gameState;

    public bool InGame { get => gameState == GameState.InGame; }
    public bool ReadingNote { get => gameState == GameState.ReadingNote; }
    public bool Paused { get => gameState == GameState.Paused; }
    public bool GameOver { get => gameState == GameState.GameOver; }
    public bool EndGame { get => gameState == GameState.EndGame; }

    private readonly float timeScalePaused = 0f;
    private readonly float timeScaleInGame = 1f;

    private void Awake() {
        InitializeSingleton();
    }
    private void Start() {
        EnterInGameState();
    }
    // in game state
    public void EnterInGameState() {
        Time.timeScale = timeScaleInGame;
        UI.HideCursor();

        gameState = GameState.InGame;
    }
    // reading note state
    public void EnterReadingNoteState() {
        Time.timeScale = timeScalePaused;

        gameState = GameState.ReadingNote;
    }
    public void ExitReadingNoteState() {
        GameUI.Notes.Hide();
    }
    // paused state
    public void EnterPausedState() {
        Time.timeScale = timeScalePaused;
        PlayerController.Instance.DisableCameraLook();
        UI.ShowCursor();
        GameUI.PausePanel.Show();

        gameState = GameState.Paused;
    }
    public void ExitPausedState() {
        PlayerController.Instance.EnableCameraLook();
        GameUI.PausePanel.Hide();
    }
    // game over state
    public void EnterGameOverState() {
        Time.timeScale = timeScalePaused;
        GameUI.GameOverPanel.Show();

        gameState = GameState.GameOver;
    }
    public void ExitGameOverState() {
        GameUI.GameOverPanel.Hide();
    }
    // end game state
    public void EnterEndGameState(EndGameVariants currentEndGame) {
        PlayerController.Instance.DisableCharacterController();
        PlayerController.Instance.DisableCameraLook();

        Time.timeScale = timeScalePaused;
        EndGamePanel.Instance.Show(currentEndGame);
    }
    public void EnterRunAwayEndGameState() {
        EnterEndGameState(EndGameVariants.Escape);

        if (SteamManager.Instance != null) {
            SteamManager.Instance.UnlockAchievement(AchievementsEnum.Runner);
        }
    }
    public void EnterBanishDemonEndGameState() {
        EnterEndGameState(EndGameVariants.BanishDemon);

        if (SteamManager.Instance != null) {
            SteamManager.Instance.UnlockAchievement(AchievementsEnum.Exorcism);
        }
    }
    // main menu state
    public void EnterMainMenuState() {
        Time.timeScale = timeScaleInGame;
        UI.ShowCursor();
    }
}
