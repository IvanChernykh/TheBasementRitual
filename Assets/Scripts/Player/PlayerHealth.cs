
public class PlayerHealth : Singleton<PlayerHealth> {

    private void Awake() {
        InitializeSingleton();
    }
    public void Kill(bool showEndGameOnKillPlayer) {
        Die(showEndGameOnKillPlayer);
    }
    private void Die(bool showEndGameOnKillPlayer) {
        if (showEndGameOnKillPlayer) {
            GameStateManager.Instance.EnterEndGameState(EndGameVariants.GiveUp);

            if (SteamManager.Instance != null) {
                SteamManager.Instance.UnlockAchievement(AchievementsEnum.GiveUp);
            }
        } else {
            GameStateManager.Instance.EnterGameOverState();
        }
    }
}
