using System.Collections;
using UnityEngine;

public class ActivateOrDeactivateObjectAction : EventAction {
    [Header("Action Settings")]
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private bool destroyObjectToHide;
    [SerializeField] private float showDelay = 0f;
    [SerializeField] private float hideDelay = 0f;

    public override void ExecuteAction() {
        if (objectToShow != null) {
            if (showDelay > 0) {
                StartCoroutine(ShowWithDelay());
            } else {
                ShowObject();
            }
        }
        if (objectToHide != null) {
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
        if (objectToHide == null) {
            return;
        }
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
