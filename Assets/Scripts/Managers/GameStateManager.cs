using UnityEngine;
using Assets.Scripts.Utils;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }
    private enum GameState {
        InGame,
        ReadingNote,
        Paused,
        GameOver
    }
    private GameState gameState;

    public bool InGame { get => gameState == GameState.InGame; }
    public bool ReadingNote { get => gameState == GameState.ReadingNote; }
    public bool Paused { get => gameState == GameState.Paused; }
    public bool GameOver { get => gameState == GameState.GameOver; }

    private readonly float timeScalePaused = 0f;
    private readonly float timeScaleInGame = 1f;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        EnterInGameState();
    }
    // in game state
    public void EnterInGameState() {
        Time.timeScale = timeScaleInGame;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameState = GameState.InGame;
    }
    // reading note state
    public void EnterReadingNoteState() {
        Time.timeScale = timeScalePaused;

        gameState = GameState.ReadingNote;
    }
    public void ExitReadingNoteState() {
        NotesUI.Instance.Hide();
    }
    // paused state
    public void EnterPausedState() {
        Time.timeScale = timeScalePaused;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PausePanel.Instance.Show();

        gameState = GameState.Paused;
    }
    public void ExitPausedState() {
        PausePanel.Instance.Hide();
    }
    // game over state
    public void EnterGameOverState() {
        Time.timeScale = timeScalePaused;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameOverPanel.Instance.Show();

        gameState = GameState.GameOver;
    }
    public void ExitGameOverState() {
        GameOverPanel.Instance.Hide();
    }
}
