using UnityEngine;

public class LookAtObject : MonoBehaviour {
    [SerializeField] private Transform targetObject;
    [SerializeField] private float angleThreshold = 1f;
    private Transform playerTransform;

    private void Start() {
        playerTransform = PlayerController.Instance.transform;
    }
    void Update() {
        if (IsPlayerLookingAtObject()) {
            Debug.Log("Looking");
        } else {
            Debug.Log("Not");
        }
    }

    bool IsPlayerLookingAtObject() {
        Vector3 directionToObject = (targetObject.position - playerTransform.position).normalized;

        float dotProduct = Vector3.Dot(playerTransform.forward, directionToObject);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        return angle < angleThreshold;
    }
}
