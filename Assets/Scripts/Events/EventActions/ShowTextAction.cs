using UnityEngine;

public class ShowTextAction : EventAction {
    [TextArea(5, 5), SerializeField] private string[] tooltipText;
    [TextArea(5, 5), SerializeField] private string[] subtitleText;
    [SerializeField] private bool showTooltip;
    [SerializeField] private bool showSubtitle;

    public override void ExecuteEvent() {
        if (showTooltip) {
            TooltipUI.Instance.Show(tooltipText);
        }
        if (showSubtitle) {
            SubtitlesUI.Instance.Show(subtitleText);
        }
    }
}
