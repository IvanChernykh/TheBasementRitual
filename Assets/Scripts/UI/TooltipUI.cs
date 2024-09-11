using UnityEngine;

public class TooltipUI : TextController {
    public static TooltipUI Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
}
