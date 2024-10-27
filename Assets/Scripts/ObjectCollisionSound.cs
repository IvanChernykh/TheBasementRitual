using UnityEngine;

public class ObjectHitSound : MonoBehaviour {
    [SerializeField] private LayerMask groundMask;
    private void OnTriggerEnter(Collider other) {
        if ((groundMask.value & (1 << other.gameObject.layer)) != 0) {
            Debug.Log("Yes");
        }
    }
}
