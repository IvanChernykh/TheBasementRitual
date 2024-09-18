using Assets.Scripts.Utils;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {
    public static GameOverPanel Instance { get; private set; }
    [SerializeField] private GameObject panel;
    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        Hide();
    }
    public void Show() {
        panel.SetActive(true);
    }
    public void Hide() {
        panel.SetActive(false);
    }
}
