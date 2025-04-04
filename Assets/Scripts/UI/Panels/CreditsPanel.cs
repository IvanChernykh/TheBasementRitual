using TMPro;
using UnityEngine;

public class CreditsPanel : Singleton<CreditsPanel> {

    [Header("Container")]
    [SerializeField] private GameObject container;

    [Header("MainMenuRefs")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject logo;

    [Header("Credits")]
    [SerializeField] private Transform textContainer;
    [SerializeField] private float textSpeed = 100f;

    [Header("Button")]
    [SerializeField] private TextMeshProUGUI backBtnText;

    private readonly Vector3 startTextPos = new Vector3(0, -440, 0);
    private readonly float endTextPosY = 1500f;

    public bool IsActive { get; private set; } = false;


    private void Awake() {
        InitializeSingleton();
    }
    private void Update() {
        if (IsActive) {
            // start over again
            if (textContainer.localPosition.y > endTextPosY) {
                textContainer.localPosition = startTextPos;
            }
            textContainer.localPosition += new Vector3(0, textSpeed * Time.deltaTime, 0);
        }
    }

    public void Show() {
        HideMainMenu();
        container.SetActive(true);
        textContainer.localPosition = startTextPos;
        IsActive = true;
    }
    public void Hide() {
        backBtnText.color = Color.white;
        IsActive = false;
        container.SetActive(false);
        ShowMainMenu();
    }

    private void ShowMainMenu() {
        mainMenu.SetActive(true);
        logo.SetActive(true);
    }
    private void HideMainMenu() {
        mainMenu.SetActive(false);
        logo.SetActive(false);
    }
}
