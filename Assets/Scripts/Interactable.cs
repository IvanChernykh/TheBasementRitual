using UnityEngine;

public class Interactable : MonoBehaviour {
    // maybe later without message
    public string interactMessage;
    public virtual void Interact() {
        Debug.LogWarning("Interactable.Interact();");
    }
}
