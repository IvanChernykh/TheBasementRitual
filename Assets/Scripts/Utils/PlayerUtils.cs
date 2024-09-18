using UnityEngine;

namespace Assets.Scripts.Utils {
    public class PlayerUtils {
        public static bool IsPlayerLookingAtObject(Transform target, float angleThreshold = 45f) {
            Vector3 directionToObject = (target.position - PlayerController.Instance.transform.position).normalized;

            float dotProduct = Vector3.Dot(PlayerController.Instance.transform.forward, directionToObject);
            float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            return angle < angleThreshold;
        }
        public static float DistanceToPlayer(Vector3 origin) {
            return Vector3.Distance(origin, PlayerController.Instance.transform.position);
        }
        public static Vector3 DirectionToPlayer(Vector3 origin) {
            return PlayerController.Instance.transform.position - origin;
        }
        public static Vector3 DirectionToPlayerNormalized(Vector3 origin) {
            return (PlayerController.Instance.transform.position - origin).normalized;
        }
    }
}
