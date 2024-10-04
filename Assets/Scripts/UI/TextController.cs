using System.Collections;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI textObject;
    [SerializeField] protected float timerMax = 2f;
    private bool coroutieRunning;

    private void Start() {
        Hide();
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
    public void Show(string[] text, float showTime) {
        if (coroutieRunning) {
            StopAllCoroutines();
        }
        textObject.gameObject.SetActive(true);
        StartCoroutine(ShowQueue(text, showTime));
    }
    public void ShowAlways(string text) {
        textObject.text = text;
        textObject.gameObject.SetActive(true);
    }
    public void Hide() {
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
    protected IEnumerator ShowQueue(string[] text, float showTime = 0) {
        coroutieRunning = true;
        foreach (string item in text) {
            textObject.text = item;
            yield return new WaitForSeconds(showTime > 0 ? showTime : timerMax);
        }
        Hide();
        coroutieRunning = false;
    }
}
