using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }
    private enum GameState {
        Playing,
        ReadingNote,
        Paused
    }
    private GameState gameState;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        gameState = GameState.Playing;
    }
    public void SetReadingNoteState() {
        gameState = GameState.ReadingNote;
    }
    public void SetPlayingState() {
        gameState = GameState.Playing;
    }
    public void SetPausedState() {
        gameState = GameState.Paused;
    }
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
