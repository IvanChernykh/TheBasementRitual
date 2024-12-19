using UnityEngine;

public class Food : Interactable {
    [Header("SteamAchievement")]
    [SerializeField] private SteamStatsEnum foodType;
    [SerializeField] private bool saveStat = false;
    private void Start() {
        interactMessage = "Eat";
    }
    protected override void Interact() {
        if (saveStat && SteamManager.Instance != null) {
            SteamManager.Instance.FoodLoverCheck(foodType);
        }
        Destroy(gameObject);
    }
}
