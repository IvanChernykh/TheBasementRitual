using UnityEngine;
using Assets.Scripts.Utils;

public class CrosshairUI : MonoBehaviour {
    public static CrosshairUI Instance { get; private set; }

    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject crosshairHovered;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        crosshairHovered.SetActive(false);
    }

    public void Hover() {
        crosshairHovered.SetActive(true);
        crosshair.SetActive(false);
    }
    public void Hide() {
        crosshair.SetActive(true);
        crosshairHovered.SetActive(false);
    }
}
