using UnityEngine;

public class ActionExecutor : Interactable {
    [SerializeField] private string interactionMessage;

    private void Start() {
        interactMessage = interactionMessage;
    }
    protected override void Interact() { }
}
