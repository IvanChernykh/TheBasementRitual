using System;
using UnityEngine;
using Assets.Scripts.Utils;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    [SerializeField] private bool isMainMenu;

    public event EventHandler OnSprintStartedEvent;
    public event EventHandler OnSprintCanceledEvent;
    public event EventHandler OnCrouchEvent;
    public event EventHandler OnJumpEvent;
    public event EventHandler OnInteractEvent;
    public event EventHandler OnFlashlightToggleEvent;
    public event EventHandler OnReloadBattery;

    private PlayerInputActions inputActions;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        if (isMainMenu) {
            inputActions.Player.Pause.performed += PausePerformed;
            return;
        }

        inputActions.Player.Sprint.started += SprintStarted;
        inputActions.Player.Sprint.canceled += SprintCanceled;
        inputActions.Player.Crouch.performed += CrouchPerformed;
        inputActions.Player.Jump.performed += JumpPerformed;
        inputActions.Player.Interact.performed += InteractPerformed;
        inputActions.Player.Flashlight.performed += FlashlightPerformed;
        inputActions.Player.ReloadBattery.performed += ReloadBatteryPerformed;
        inputActions.Player.Pause.performed += PausePerformed;
    }
    private void OnDestroy() {
        if (isMainMenu) {
            inputActions.Player.Pause.performed -= PausePerformed;

            inputActions.Dispose();
            return;
        }
        inputActions.Player.Sprint.started -= SprintStarted;
        inputActions.Player.Sprint.canceled -= SprintCanceled;
        inputActions.Player.Crouch.performed -= CrouchPerformed;
        inputActions.Player.Jump.performed -= JumpPerformed;
        inputActions.Player.Interact.performed -= InteractPerformed;
        inputActions.Player.Flashlight.performed -= FlashlightPerformed;
        inputActions.Player.ReloadBattery.performed -= ReloadBatteryPerformed;
        inputActions.Player.Pause.performed -= PausePerformed;

        inputActions.Dispose();
    }
    // events
    private void SprintStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnSprintStartedEvent?.Invoke(this, EventArgs.Empty);
    }
    private void SprintCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintCanceledEvent?.Invoke(this, EventArgs.Empty);
    }
    private void CrouchPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnCrouchEvent?.Invoke(this, EventArgs.Empty);
    }
    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnJumpEvent?.Invoke(this, EventArgs.Empty);
    }
    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnInteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void FlashlightPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnFlashlightToggleEvent?.Invoke(this, EventArgs.Empty);
    }
    private void ReloadBatteryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnReloadBattery?.Invoke(this, EventArgs.Empty);
    }
    private void PausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (isMainMenu) {
            if (OptionsPanel.Instance.IsActive) {
                OptionsPanel.Instance.Hide();
            }
            if (CreditsPanel.Instance.IsActive) {
                CreditsPanel.Instance.Hide();
            }
            return;
        }
        if (GameStateManager.Instance.InGame) {
            GameStateManager.Instance.EnterPausedState();
            return;
        }
        if (GameStateManager.Instance.Paused) {
            if (OptionsPanel.Instance.IsActive) {
                OptionsPanel.Instance.Hide();
            } else {
                GameStateManager.Instance.ExitPausedState();
                GameStateManager.Instance.EnterInGameState();
            }
            return;
        }
        if (GameStateManager.Instance.ReadingNote) {
            GameStateManager.Instance.ExitReadingNoteState();
            GameStateManager.Instance.EnterInGameState();
        }
    }
    // helpers
    public Vector2 GetMovementVector() {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetMovementVectorNormalized() {
        Vector2 input = inputActions.Player.Movement.ReadValue<Vector2>();
        return input.normalized;
    }
    public Vector2 GetMouseVector() {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }
}
