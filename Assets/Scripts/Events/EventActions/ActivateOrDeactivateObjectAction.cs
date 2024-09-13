using System.Collections;
using UnityEngine;

public class ActivateOrDeactivateObjectAction : EventAction {
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private bool destroyObjectToHide;
    [SerializeField] private float showDelay = 0f;
    [SerializeField] private float hideDelay = 0f;

    public override void ExecuteEvent() {
        if (objectToShow != null) {
            if (showDelay > 0) {
                StartCoroutine(ShowWithDelay());
            } else {
                ShowObject();
            }
        }
        if (objectToHide != null && objectToHide.activeInHierarchy) {
            if (hideDelay > 0) {
                StartCoroutine(HideWithDelay());
            } else {
                HideObject();
            }
        }
    }
    private void ShowObject() {
        objectToShow.SetActive(true);
    }
    private void HideObject() {
        if (destroyObjectToHide) {
            Destroy(objectToHide);
        } else {
            objectToHide.SetActive(false);
        }
    }
    private IEnumerator ShowWithDelay() {
        yield return new WaitForSeconds(showDelay);
        ShowObject();
    }
    private IEnumerator HideWithDelay() {
        yield return new WaitForSeconds(hideDelay);
        HideObject();
    }
}
