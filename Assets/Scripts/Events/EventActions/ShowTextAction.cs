using System.Collections;
using UnityEngine;

public class ShowTextAction : EventAction {
    [TextArea(5, 5), SerializeField] private string[] tooltipText;
    [TextArea(5, 5), SerializeField] private string[] subtitleText;
    [SerializeField] private float tooltipDelay = 0f;
    [SerializeField] private float subtitleDelay = 0f;
    [SerializeField] private bool showTooltip;
    [SerializeField] private bool showSubtitle;

    public override void ExecuteEvent() {
        if (showTooltip) {
            if (tooltipDelay > 0) {
                StartCoroutine(ShowTooltipWithDelay());
            } else {
                TooltipUI.Instance.Show(tooltipText);
            }
        }
        if (showSubtitle) {
            if (subtitleDelay > 0) {
                StartCoroutine(ShowSubtitleWithDelay());
            } else {
                SubtitlesUI.Instance.Show(subtitleText);
            }

        }
    }
    private IEnumerator ShowTooltipWithDelay() {
        yield return new WaitForSeconds(tooltipDelay);
        TooltipUI.Instance.Show(tooltipText);
    }
    private IEnumerator ShowSubtitleWithDelay() {
        yield return new WaitForSeconds(tooltipDelay);
        SubtitlesUI.Instance.Show(tooltipText);
    }
}
