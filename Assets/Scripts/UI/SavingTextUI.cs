using System.Collections;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

public class SavingTextUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI saveText;
    private readonly float fadeTime = .2f;

    private void Start() {
        saveText.gameObject.SetActive(false);
    }

    public void Show() {
        StartCoroutine(ShowUI());
    }
    private IEnumerator ShowUI() {
        saveText.gameObject.SetActive(true);
        yield return StartCoroutine(UI.FadeGraphic(saveText, fadeTime, fadeIn: true));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(UI.FadeGraphic(saveText, fadeTime, fadeIn: false));
        saveText.gameObject.SetActive(false);
    }
}
