using Assets.Scripts.Utils;

public class LockerClosed : Interactable {
    private void Start() {
        interactMessage = "Hide";
    }
    protected override void Interact() {
        GameUI.Tooltip.Show(LocalizationHelper.LocalizeTooltip("Locked"));
        DoorAudio.Instance.PlayLocked(transform.position, .5f);
    }
}
