using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    private PlayerInputActions inputActions;
    private void Awake() {
        Instance = this;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }
    private void OnDestroy() {
        inputActions.Dispose();
    }
    public Vector2 GetMovementVectorNormalized() {
        Vector2 input = inputActions.Player.Movement.ReadValue<Vector2>();
        return input.normalized;
    }
    public Vector2 GetMouseVector() {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }
}
