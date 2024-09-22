using UnityEngine;

public class LockerClosed : Interactable {
    private void Start() {
        interactMessage = "Hide";
    }
    protected override void Interact() {
        TooltipUI.Instance.Show("Locked");
        DoorAudio.Instance.PlayLocked(transform.position, .5f);
    }
}
