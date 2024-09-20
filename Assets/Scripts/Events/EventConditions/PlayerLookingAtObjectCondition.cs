using Assets.Scripts.Utils;
using UnityEngine;

public class PlayerLookingAtObjectCondition : EventCondition {
    [SerializeField] private Transform targetObject;
    public override bool IsConditionMet() {
        return PlayerUtils.IsPlayerLookingAtObject(targetObject);
    }
}
