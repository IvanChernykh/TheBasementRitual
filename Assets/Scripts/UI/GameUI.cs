using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : Singleton<GameUI> {
    [Header("Components")]
    [SerializeField] private BatteryUI batteryUI;
    [SerializeField] private CrosshairUI crosshairUI;
    [SerializeField] private InteractionMessageUI interactionMessageUI;
    [SerializeField] private SubtitlesUI subtitlesUI;
    [SerializeField] private TooltipUI tooltipUI;
    [SerializeField] private NotesUI notesUI;
    [SerializeField] private SavingTextUI savingTextUI;

    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private GameOverPanel gameOverPanel;

    public static BatteryUI Battery => Instance.batteryUI;
    public static CrosshairUI Crosshair => Instance.crosshairUI;
    public static InteractionMessageUI InteractionMessage => Instance.interactionMessageUI;
    public static SubtitlesUI Subtitles => Instance.subtitlesUI;
    public static TooltipUI Tooltip => Instance.tooltipUI;
    public static NotesUI Notes => Instance.notesUI;
    public static SavingTextUI SavingText => Instance.savingTextUI;

    public static PausePanel PausePanel => Instance.pausePanel;
    public static GameOverPanel GameOverPanel => Instance.gameOverPanel;

    private void Awake() {
        InitializeSingleton();
    }
}
