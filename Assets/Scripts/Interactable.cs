using UnityEngine;

public class Interactable : MonoBehaviour {
    public string interactMessage { get; protected set; }
    public virtual void Interact() {
        Debug.LogWarning("Interactable.Interact();");
    }
}
