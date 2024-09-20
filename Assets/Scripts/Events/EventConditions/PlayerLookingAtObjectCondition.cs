using Assets.Scripts.Utils;
using UnityEngine;

public class PlayerLookingAtObjectCondition : EventCondition {
    [SerializeField] private Transform targetObject;
    [SerializeField] private float angleTreshhold = 45f;
    public override bool IsConditionMet() {
        return PlayerUtils.IsPlayerLookingAtObject(targetObject, angleTreshhold);
    }
}
