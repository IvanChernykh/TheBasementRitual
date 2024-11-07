using TMPro;
using UnityEngine;
using Assets.Scripts.Utils;
using UnityEngine.Localization;

public class InteractionMessageUI : MonoBehaviour {
    public static InteractionMessageUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI message;

    private LocalizedString localizedMessage;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start() {
        gameObject.SetActive(false);
    }

    private void OnMessageChanged(string localizedText) {
        message.text = localizedText;
        gameObject.SetActive(true);

        localizedMessage.StringChanged -= OnMessageChanged;
    }

    public void Show(string textKey) {
        if (string.IsNullOrEmpty(textKey)) return;

        localizedMessage = new LocalizedString {
            TableReference = LocalizationTables.Interactions,
            TableEntryReference = textKey
        };

        localizedMessage.StringChanged += OnMessageChanged;
        localizedMessage.RefreshString();
    }
    public void Hide() {
        gameObject.SetActive(false);
        message.text = "";
    }
}
