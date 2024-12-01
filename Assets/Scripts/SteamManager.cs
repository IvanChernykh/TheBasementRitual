using Assets.Scripts.Utils;
using UnityEngine;

public enum AchievementsEnum {
    GiveUp,
    Runner,
    Exorcism,
    FoodLover,
}

public enum SteamStatsEnum {
    ChipsEaten,
    PizzaEaten,
    PeanutsEaten
}

public class SteamManager : MonoBehaviour {
    public static SteamManager Instance { get; private set; }

    private readonly uint appId = 3285930;

    private bool connectedToSteam = false;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        try {
            Steamworks.SteamClient.Init(appId);
            connectedToSteam = true;
        }
        catch (System.Exception e) {
            Debug.Log(e);
            connectedToSteam = false;
        }
    }

    private void OnDestroy() {
        if (Instance == this) {
            Disconnect();
        }
    }

    private void Update() {
        if (connectedToSteam) {
            Steamworks.SteamClient.RunCallbacks();
        }
    }

    private Steamworks.Data.Achievement GetAchievement(AchievementsEnum achievement) {
        return new Steamworks.Data.Achievement(achievement.ToString());
    }

    public void Disconnect() {
        if (connectedToSteam) {
            Steamworks.SteamClient.Shutdown();
            connectedToSteam = false;
        }
    }

    public void UnlockAchievement(AchievementsEnum achievement) {
        if (connectedToSteam) {
            var ach = GetAchievement(achievement);
            if (!ach.State) {
                ach.Trigger();
            }
        }
    }
    public void ResetAchievement(AchievementsEnum achievement) {
        if (connectedToSteam) {
            var ach = GetAchievement(achievement);
            ach.Clear();
        }
    }

    public void ResetAllAchievementsAndStats() {
        Steamworks.SteamUserStats.ResetAll(true);
    }

    public void FoodLoverCheck(SteamStatsEnum foodType) {
        if (!connectedToSteam) {
            return;
        }
        var ach = GetAchievement(AchievementsEnum.FoodLover);
        if (ach.State) {
            return;
        }

        Steamworks.SteamUserStats.AddStat(foodType.ToString(), 1);

        int chips = Steamworks.SteamUserStats.GetStatInt(SteamStatsEnum.ChipsEaten.ToString());
        int pizza = Steamworks.SteamUserStats.GetStatInt(SteamStatsEnum.PizzaEaten.ToString());
        int nuts = Steamworks.SteamUserStats.GetStatInt(SteamStatsEnum.PeanutsEaten.ToString());

        if (chips > 0 && pizza > 0 && nuts > 0) {
            UnlockAchievement(AchievementsEnum.FoodLover);
        }
    }
}
