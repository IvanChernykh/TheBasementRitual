using UnityEngine;

public class ShowText : OnTriggerEnterBase {
    [TextArea(5, 5), SerializeField] private string[] tooltipText;
    [TextArea(5, 5), SerializeField] private string[] subtitleText;
    [SerializeField] private bool showTooltip;
    [SerializeField] private bool showSubtitle;

    protected override void HandleEvent() {
        if (showTooltip) {
            TooltipUI.Instance.Show(tooltipText);
        }
        if (showSubtitle) {
            SubtitlesUI.Instance.Show(subtitleText);
        }
    }
}
