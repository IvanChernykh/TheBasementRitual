using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public string interactMessage { get; protected set; }
    abstract public void Interact();
}
