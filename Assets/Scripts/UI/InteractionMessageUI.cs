using TMPro;
using UnityEngine;
using Assets.Scripts.Utils;

public class InteractionMessageUI : MonoBehaviour {
    public static InteractionMessageUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI message;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        gameObject.SetActive(false);
    }
    public void Show(string text) {
        gameObject.SetActive(true);
        message.text = text;
    }
    public void Hide() {
        gameObject.SetActive(false);
        message.text = "";
    }
}
