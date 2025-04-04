using UnityEngine;

public class CrosshairUI : MonoBehaviour {

    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject crosshairHovered;

    private void Start() {
        crosshairHovered.SetActive(false);
    }

    public void Hover() {
        crosshairHovered.SetActive(true);
        crosshair.SetActive(false);
    }
    public void StopHover() {
        crosshair.SetActive(true);
        crosshairHovered.SetActive(false);
    }
}
