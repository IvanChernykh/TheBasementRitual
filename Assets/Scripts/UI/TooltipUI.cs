using UnityEngine;
using Assets.Scripts.Utils;

public class TooltipUI : TextController {
    public static TooltipUI Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
