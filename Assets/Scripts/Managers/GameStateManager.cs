using UnityEngine;
using Assets.Scripts.Utils;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }
    private enum GameState {
        Playing,
        ReadingNote,
        Paused
    }
    private GameState gameState;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        SetPlayingState();
    }
    // setters
    public void SetReadingNoteState() {
        Time.timeScale = 0f;
        gameState = GameState.ReadingNote;
    }
    public void SetPlayingState() {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameState = GameState.Playing;
    }
    public void SetPausedState() {
        Time.timeScale = 0f;
        gameState = GameState.Paused;
    }
    // getters
    public bool IsReadingNote() {
        return gameState == GameState.ReadingNote;
    }
    public bool IsPlaying() {
        return gameState == GameState.Playing;
    }
    public bool IsPaused() {
        return gameState == GameState.Paused;
    }
}
