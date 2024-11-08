using Assets.Scripts.Utils;
using UnityEngine;

public class LockerClosed : Interactable {
    private void Start() {
        interactMessage = "Hide";
    }
    protected override void Interact() {
        TooltipUI.Instance.Show(LocalizationHelper.LocalizeTooltip("Locked"));
        DoorAudio.Instance.PlayLocked(transform.position, .5f);
    }
}
