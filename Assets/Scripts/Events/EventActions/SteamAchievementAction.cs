using UnityEngine;

public class SteamAchievementAction : EventAction {
    [SerializeField] private AchievementsEnum achievementToUnlock;

    public override void ExecuteAction() {
        if (SteamManager.Instance != null) {
            SteamManager.Instance.UnlockAchievement(achievementToUnlock);
        }
    }
}
