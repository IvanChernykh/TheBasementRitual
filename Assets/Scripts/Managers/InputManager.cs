using System;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }
    public event EventHandler OnSprintStartedEvent;
    public event EventHandler OnSprintCanceledEvent;
    public event EventHandler OnCrouchEvent;
    public event EventHandler OnJumpEvent;
    public event EventHandler OnInteractEvent;
    public event EventHandler OnFlashlightToggleEvent;
    private PlayerInputActions inputActions;

    private void Awake() {
        Instance = this;

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Sprint.started += SprintStarted;
        inputActions.Player.Sprint.canceled += SprintCanceled;
        inputActions.Player.Crouch.performed += CrouchPerformed;
        inputActions.Player.Jump.performed += JumpPerformed;
        inputActions.Player.Interact.performed += InteractPerformed;
        inputActions.Player.Flashlight.performed += FlashlightPerformed;
    }
    private void OnDestroy() {
        inputActions.Player.Sprint.started -= SprintStarted;
        inputActions.Player.Sprint.canceled -= SprintCanceled;
        inputActions.Player.Crouch.performed -= CrouchPerformed;
        inputActions.Player.Jump.performed -= JumpPerformed;
        inputActions.Player.Interact.performed -= InteractPerformed;
        inputActions.Player.Flashlight.performed -= FlashlightPerformed;
        inputActions.Dispose();
    }

    private void SprintStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintStartedEvent?.Invoke(this, EventArgs.Empty);
    }
    private void SprintCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintCanceledEvent?.Invoke(this, EventArgs.Empty);
    }
    private void CrouchPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnCrouchEvent?.Invoke(this, EventArgs.Empty);
    }
    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpEvent?.Invoke(this, EventArgs.Empty);
    }
    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void FlashlightPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnFlashlightToggleEvent?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 input = inputActions.Player.Movement.ReadValue<Vector2>();
        return input.normalized;
    }
    public Vector2 GetMouseVector() {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }
}
