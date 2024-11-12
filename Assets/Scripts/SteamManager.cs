using Assets.Scripts.Utils;
using UnityEngine;

public enum AchievementsEnum {
    GiveUp,
    Runner,
    Exorcizm,
    FoodLover,
}

public enum SteamStatsEnum {
    ChipsEaten,
    PizzaEaten,
    PanutsEaten
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

    public void Disconnect() {
        if (connectedToSteam) {
            Steamworks.SteamClient.Shutdown();
            connectedToSteam = false;
        }
    }

    public void UnlockAchievement(AchievementsEnum achievement) {
        if (connectedToSteam) {
            var ach = new Steamworks.Data.Achievement(achievement.ToString());
            if (!ach.State) {
                ach.Trigger();
            }
        }
    }
    public void ResetAchievement(AchievementsEnum achievement) {
        if (connectedToSteam) {
            var ach = new Steamworks.Data.Achievement(achievement.ToString());
            ach.Clear();
        }
    }

    public void FoodLoverCheck() {
        // int foodConsumed = Steamworks.SteamUserStats.GetStatInt(SteamStatsEnum.FoodConsumed.ToString());
        // if (foodConsumed == 3) {
        //     return;
        // }
        // foodConsumed++;
        // Steamworks.SteamUserStats.SetStat(SteamStatsEnum.FoodConsumed.ToString(), foodConsumed);

        // if (foodConsumed == 3) {
        //     UnlockAchievement(AchievementsEnum.FoodLover);
        // }
    }
}
