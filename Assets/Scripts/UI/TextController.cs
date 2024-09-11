using System.Collections;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI textObject;
    [SerializeField] protected float timerMax = 2f;
    private bool coroutieRunning;

    private void Start() {
        textObject.text = "";
        textObject.gameObject.SetActive(false);
    }
    public void Show(string text) {
        if (coroutieRunning) {
            StopAllCoroutines();
        }
        textObject.gameObject.SetActive(true);
        StartCoroutine(ShowSingle(text));
    }
    public void Show(string[] text) {
        if (coroutieRunning) {
            StopAllCoroutines();
        }
        textObject.gameObject.SetActive(true);
        StartCoroutine(ShowQueue(text));
    }
    protected void Hide() {
        textObject.text = "";
        textObject.gameObject.SetActive(false);
    }
    protected IEnumerator ShowSingle(string text) {
        coroutieRunning = true;
        textObject.text = text;
        yield return new WaitForSeconds(timerMax);
        Hide();
        coroutieRunning = false;
    }
    protected IEnumerator ShowQueue(string[] text) {
        coroutieRunning = true;
        foreach (string item in text) {
            textObject.text = item;
            yield return new WaitForSeconds(timerMax);
        }
        Hide();
        coroutieRunning = false;
    }
}
