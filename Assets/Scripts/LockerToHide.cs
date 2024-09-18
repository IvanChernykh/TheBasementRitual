using UnityEngine;

public class LockerToHide : Interactable {
    [SerializeField] private Transform hidePosition;
    [SerializeField] private Transform exitPosition;
    // private MeshRenderer meshRenderer;
    private PlayerController player;
    private void Awake() {
        interactMessage = "Hide";
    }
    private void Start() {
        // meshRenderer = GetComponent<MeshRenderer>();
        player = PlayerController.Instance;
    }
    private void Update() {
        if (player.isHiding) {
            interactMessage = "Exit";
        } else {
            interactMessage = "Hide";
        }
    }
    protected override void Interact() {
        if (player.isHiding) {
            Exit();
        } else {
            Hide();
        }

    }
    private void Hide() {
        player.Hide();
        Debug.Log(hidePosition.position);
        Debug.Log(hidePosition.rotation);
        player.transform.position = hidePosition.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, hidePosition.eulerAngles.y - 180, 0));
        // meshRenderer.enabled = false;
    }
    private void Exit() {
        player.transform.position = exitPosition.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        player.UnHide();
    }
}
