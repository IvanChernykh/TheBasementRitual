using UnityEngine;

namespace Assets.Scripts.Utils {
    public class Exceptions {
        public static void MoreThanOneInstance(string instanceName) {
            Debug.LogError($"There is more than one instance of {instanceName}");
        }
    }
}