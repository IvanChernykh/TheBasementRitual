using UnityEngine;

public class Interactable : MonoBehaviour {
    public virtual void Interact() {
        Debug.LogWarning("Interactable.Interact();");
    }
}
