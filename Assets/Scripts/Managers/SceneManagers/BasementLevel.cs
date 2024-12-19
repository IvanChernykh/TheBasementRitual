using UnityEngine;

public class BasementLevel : MonoBehaviour {
    private void Start() {
        if (SteamManager.Instance != null) {
            SteamManager.Instance.UnlockAchievement(AchievementsEnum.WakeUp);
        }
    }
}
