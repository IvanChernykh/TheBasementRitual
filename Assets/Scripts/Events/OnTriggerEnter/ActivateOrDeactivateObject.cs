using UnityEngine;

public class ActivateOrDeactivateObject : OnTriggerEnterBase {
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private GameObject objectToHide;

    protected override void HandleEvent() {
        if (objectToShow != null) {
            objectToShow.SetActive(true);
        }
        if (objectToHide != null) {
            objectToHide.SetActive(false);
        }
    }
}
