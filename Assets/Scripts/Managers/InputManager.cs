using System;
using UnityEngine;

public class InputManager : Singleton<InputManager> {

    [SerializeField] private bool isMainMenu;

    public event EventHandler OnSprintStartedEvent;
    public event EventHandler OnSprintCanceledEvent;
    public event EventHandler OnCrouchEvent;
    public event EventHandler OnInteractEvent;
    public event EventHandler OnFlashlightToggleEvent;
    public event EventHandler OnReloadBattery;
    public event EventHandler OnMouseScroll;
    public event EventHandler OnInventoryEvent;
    public event EventHandler OnInventoryCanceledEvent;

    private PlayerInputActions inputActions;

    public bool CanPause { get; private set; } = true;

    private void Awake() {
        InitializeSingleton();

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        if (isMainMenu) {
            inputActions.Player.Pause.performed += PausePerformed;
            return;
        }

        inputActions.Player.Sprint.started += SprintStarted;
        inputActions.Player.Sprint.canceled += SprintCanceled;
        inputActions.Player.Crouch.performed += CrouchPerformed;
        inputActions.Player.Interact.performed += InteractPerformed;
        inputActions.Player.Flashlight.performed += FlashlightPerformed;
        inputActions.Player.ReloadBattery.performed += ReloadBatteryPerformed;
        inputActions.Player.Pause.performed += PausePerformed;
        inputActions.Player.MouseWheel.performed += MouseScrollPerformed;
        inputActions.Player.Inventory.performed += InventoryPerformed;
        inputActions.Player.Inventory.canceled += InventoryCanceled;
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
        inputActions.Player.Interact.performed -= InteractPerformed;
        inputActions.Player.Flashlight.performed -= FlashlightPerformed;
        inputActions.Player.ReloadBattery.performed -= ReloadBatteryPerformed;
        inputActions.Player.Pause.performed -= PausePerformed;
        inputActions.Player.MouseWheel.performed -= MouseScrollPerformed;
        inputActions.Player.Inventory.performed -= InventoryPerformed;
        inputActions.Player.Inventory.canceled -= InventoryCanceled;
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
    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnInteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void InventoryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnInventoryEvent?.Invoke(this, EventArgs.Empty);
    }
    private void InventoryCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!GameStateManager.Instance.InGame) {
            return;
        }
        OnInventoryCanceledEvent?.Invoke(this, EventArgs.Empty);
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
            if (CanPause) {
                GameStateManager.Instance.EnterPausedState();
                return;
            }
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
    private void MouseScrollPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMouseScroll?.Invoke(this, EventArgs.Empty);
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
    public float GetMouseScrollValue() {
        return inputActions.Player.MouseWheel.ReadValue<float>();
    }
    public void SetCanPause(bool canPause) {
        CanPause = canPause;
    }
}
