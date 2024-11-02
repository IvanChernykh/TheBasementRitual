using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public static PlayerHealth Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void Kill(bool showEndGameOnKillPlayer) {
        Die(showEndGameOnKillPlayer);
    }
    private void Die(bool showEndGameOnKillPlayer) {
        if (showEndGameOnKillPlayer) {
            GameStateManager.Instance.EnterEndGameState(EndGameVariants.GiveUp);
        } else {
            GameStateManager.Instance.EnterGameOverState();
        }
    }
}
