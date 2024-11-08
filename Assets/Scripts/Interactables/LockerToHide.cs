using Assets.Scripts.Utils;
using UnityEngine;

public class LockerToHide : Interactable {
    [SerializeField] private Transform hidePosition;
    [SerializeField] private Transform exitPosition;
    [SerializeField] private Transform rotationPoint;
    private PlayerController player;
    private readonly float maxPeekAngle = 12f;
    private readonly float peekSpeed = 16f;
    private float currentRotation;
    private Quaternion defaultRotation;
    private bool hidingHere; // check if hiding in exact locker

    private void Awake() {
        interactMessage = "Hide";
    }
    private void Start() {
        defaultRotation = transform.rotation;
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
        player.RestrictRotation(40f);
        player.transform.position = hidePosition.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, hidePosition.eulerAngles.y - 180, 0));
        TooltipUI.Instance.ShowAlways(LocalizationHelper.LocalizeTooltip("Peek - [ W / S ]"));
    }
    private void Exit() {
        TooltipUI.Instance.Hide();
        currentRotation = 0;
        transform.rotation = defaultRotation;
        player.transform.position = exitPosition.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, hidePosition.eulerAngles.y - 180, 0));
        player.UnHide();
        player.UnrestrictRotation();
    }
    private void HandlePeek() {
        Vector2 inputVector = InputManager.Instance.GetMovementVector();
        float directionMultiplier = -Mathf.Sign(transform.lossyScale.x);

        if (inputVector.y > 0) {
            float rotationStep = peekSpeed * Time.deltaTime;
            if (currentRotation < maxPeekAngle) {
                transform.RotateAround(rotationPoint.position, Vector3.up, rotationStep * directionMultiplier);
                currentRotation += rotationStep;

                if (currentRotation > maxPeekAngle) {
                    float excessRotation = currentRotation - maxPeekAngle;
                    transform.RotateAround(rotationPoint.position, Vector3.up, -excessRotation * directionMultiplier);
                    currentRotation = maxPeekAngle;
                }
            }
        }
        if (inputVector.y < 0) {
            float rotationStep = -peekSpeed * Time.deltaTime;
            if (currentRotation > 0) {
                transform.RotateAround(rotationPoint.position, Vector3.up, rotationStep * directionMultiplier);
                currentRotation += rotationStep;

                if (currentRotation < 0) {
                    float excessRotation = 0 - currentRotation;
                    transform.RotateAround(rotationPoint.position, Vector3.up, excessRotation * directionMultiplier);
                    currentRotation = 0;
                }
            }
        }
    }
}
