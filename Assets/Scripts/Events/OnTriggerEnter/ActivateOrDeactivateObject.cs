using System.Collections;
using UnityEngine;

public class ActivateOrDeactivateObject : OnTriggerEnterBase {
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private float showDelay = 0f;
    [SerializeField] private float hideDelay = 0f;

    protected override void HandleEvent() {
        if (objectToShow != null) {
            if (showDelay > 0) {
                StartCoroutine(ShowWithDelay());
            } else {
                objectToShow.SetActive(true);
            }
        }
        if (objectToHide != null) {
            if (hideDelay > 0) {
                StartCoroutine(HideWithDelay());
            } else {
                objectToHide.SetActive(false);
            }
        }
    }
    private IEnumerator ShowWithDelay() {
        yield return new WaitForSeconds(showDelay);
        objectToShow.SetActive(true);
    }
    private IEnumerator HideWithDelay() {
        yield return new WaitForSeconds(hideDelay);
        objectToHide.SetActive(false);
    }
}
