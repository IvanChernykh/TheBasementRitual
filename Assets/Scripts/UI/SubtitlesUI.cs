using UnityEngine;

public class SubtitlesUI : TextController {
    public static SubtitlesUI Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
}
