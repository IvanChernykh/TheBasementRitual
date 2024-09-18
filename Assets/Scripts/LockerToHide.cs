using UnityEngine;

public class LockerToHide : Interactable {
    [SerializeField] private Transform hidePosition;
    [SerializeField] private Transform exitPosition;
    [SerializeField] private Transform rotationPoint;
    private PlayerController player;
    private float maxPeekAngle = 12f;
    private float peekSpeed = 15f;
    private float currentRotation;
    private bool hidingHere; // check if hiding in exact locker

    private void Awake() {
        interactMessage = "Hide";
    }
    private void Start() {
        player = PlayerController.Instance;
    }
    private void Update() {
        if (player.isHiding && hidingHere) {
            HandlePeek();
        }
    }
    protected override void Interact() {
        if (player.isHiding) {
            Exit();
            hidingHere = false;
            interactMessage = "Hide";
        } else {
            Hide();
            hidingHere = true;
            interactMessage = "Exit";
        }

    }
    private void Hide() {
        player.Hide();
        player.transform.position = hidePosition.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, hidePosition.eulerAngles.y - 180, 0));
        TooltipUI.Instance.ShowAlways("Peek - [ W / S ]");
    }
    private void Exit() {
        TooltipUI.Instance.Hide();
        currentRotation = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        player.transform.position = exitPosition.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, hidePosition.eulerAngles.y - 180, 0));
        player.UnHide();
    }
    private void HandlePeek() {
        Vector2 inputVector = InputManager.Instance.GetMovementVector();

        if (inputVector.y > 0) {
            float rotationStep = peekSpeed * Time.deltaTime;

            if (currentRotation < maxPeekAngle) {
                transform.RotateAround(rotationPoint.position, Vector3.up, rotationStep);
                currentRotation += rotationStep;

                if (currentRotation > maxPeekAngle) {
                    float excessRotation = currentRotation - maxPeekAngle;
                    transform.RotateAround(rotationPoint.position, Vector3.up, -excessRotation);

                    currentRotation = maxPeekAngle;
                }
            }
        }
        if (inputVector.y < 0) {
            float rotationStep = -peekSpeed * Time.deltaTime;
            if (currentRotation > 0) {
                transform.RotateAround(rotationPoint.position, Vector3.up, rotationStep); currentRotation += rotationStep;
                if (currentRotation < 0) {
                    float excessRotation = 0 - currentRotation;
                    transform.RotateAround(rotationPoint.position, Vector3.up, excessRotation);
                    currentRotation = 0;
                }
            }
        }
    }
}
