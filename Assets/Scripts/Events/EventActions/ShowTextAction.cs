using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Localization;

public class ShowTextAction : EventAction {
    [Header("Action Settings")]
    [TextArea(5, 5), SerializeField] private string[] tooltipText;
    [TextArea(5, 5), SerializeField] private string[] subtitleText;
    [SerializeField] private float tooltipDelay = 0f;
    [SerializeField] private float subtitleDelay = 0f;
    [SerializeField] private float subtitleShowTime = 3f;
    [SerializeField] private float tooltipShowTime = 0f;
    [SerializeField] private bool showTooltip;
    [SerializeField] private bool showSubtitle;

    private List<string> tooltipTextCache = new List<string>();
    private List<string> subtitleTextCache = new List<string>();

    private void InitializeLocalizedStrings() {
        tooltipTextCache = new List<string>();
        foreach (var key in tooltipText) {
            var localizedString = new LocalizedString { TableReference = LocalizationTables.Tooltips, TableEntryReference = key };
            tooltipTextCache.Add(localizedString.GetLocalizedString());
        }

        subtitleTextCache = new List<string>();
        foreach (var key in subtitleText) {
            var localizedString = new LocalizedString { TableReference = LocalizationTables.Subtitles, TableEntryReference = key };
            subtitleTextCache.Add(localizedString.GetLocalizedString());
        }
    }

    public override void ExecuteAction() {
        InitializeLocalizedStrings();
        if (showTooltip) {
            if (tooltipDelay > 0) {
                StartCoroutine(ShowTooltipWithDelay());
            } else {
                ShowTooltip();
            }
        }
        if (showSubtitle) {
            if (subtitleDelay > 0) {
                StartCoroutine(ShowSubtitleWithDelay());
            } else {
                ShowSubtitle();
            }

        }
    }
    private IEnumerator ShowTooltipWithDelay() {
        yield return new WaitForSeconds(tooltipDelay);
        ShowTooltip();
    }
    private IEnumerator ShowSubtitleWithDelay() {
        yield return new WaitForSeconds(subtitleDelay);
        ShowSubtitle();

    }

    private void ShowSubtitle() {
        if (subtitleTextCache.Count > 0) {
            GameUI.Subtitles.Show(subtitleTextCache.ToArray(), subtitleShowTime);
        } else {
            GameUI.Subtitles.Show(subtitleText, subtitleShowTime);
        }
    }
    private void ShowTooltip() {
        if (tooltipTextCache.Count > 0) {
            GameUI.Tooltip.Show(tooltipTextCache.ToArray(), tooltipShowTime);
        } else {
            GameUI.Tooltip.Show(tooltipText, tooltipShowTime);
        }
    }
}
