using UnityEngine;
using Assets.Scripts.Utils;

public class SubtitlesUI : TextController {
    public static SubtitlesUI Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
