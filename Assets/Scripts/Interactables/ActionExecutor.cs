using UnityEngine;

public class ActionExecutor : Interactable {
    [SerializeField] private string interactionMessage;
    [Tooltip("Make non interactable after executing action")]
    [SerializeField] private bool makeNonInteractable;

    private void Start() {
        interactMessage = interactionMessage;
    }
    protected override void Interact() {
        if (makeNonInteractable) {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
