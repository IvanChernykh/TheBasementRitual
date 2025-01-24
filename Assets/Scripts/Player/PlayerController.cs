using UnityEngine;
using Assets.Scripts.Utils;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }
    private CharacterController controller;
    private float normalHeight = 1.8f;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 4f;
    [SerializeField] private float crouchSpeed = 1.5f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight;
    private readonly float crouchTransitionSpeed = 3f;

    [Header("Ground Checking")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    [Header("Mouse")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform flashlightTransform;
    [SerializeField] private float mouseSensivity = 10f;
    private float rotationY;
    private float rotationX;
    private Vector2 flashlightRotation = Vector2.zero;
    private float maxRotationX = 0; // unlimited if 0
    private float maxRotationY = 85f;
    private readonly float flashlightRotSensFactor = 4f; // divide mouse sens by this num
    private readonly float flashlightRotationMax = 3f;
    public readonly float defaultMaxRotationY = 85f;
    public readonly float defaultMaxRotationX = 0f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.8f * 2;
    private Vector3 velocity = Vector3.zero;

    [Header("Interactions")]
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private LayerMask nonInteractableMask;
    public float interactDistance { get; private set; } = 2f;

    // flags
    public bool isMoving { get; private set; }
    public bool isSprinting { get; private set; }
    public bool isLanding { get; private set; }
    public bool isCrouching { get; private set; }
    public bool isHiding { get; private set; }
    public bool inChase { get; private set; }
    private bool isGrounded;
    private bool isFloating;
    private bool isInteracting;

    private bool cameraLookEnabled = true;
    private bool canSprint { get => isGrounded && !isCrouching && !isHiding; }
    private bool canCrouch { get => isGrounded && !isCrouching || isGrounded && isCrouching && canStandUp; }
    public bool canStandUp { get; set; } = true;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        controller = GetComponent<CharacterController>();
    }
    private void Start() {
        InputManager.Instance.OnSprintStartedEvent += OnSprintStarted;
        InputManager.Instance.OnSprintCanceledEvent += OnSprintCanceled;
        InputManager.Instance.OnCrouchEvent += OnCrouch;
        InputManager.Instance.OnInteractEvent += OnInteract;
    }
    private void Update() {
        if (!isHiding) {
            isGrounded = IsGrounded();
            if (isGrounded && velocity.y < 0) {
                velocity.y = -2;
            }
            HandleMovement();
            HandleCrouch();
        }
        HandleInteraction();
        CheckIsLandingAndIsFloating();
    }
    private void LateUpdate() {
        HandleCamera();
    }
    // handlers
    private void HandleCamera() {
        if (!cameraLookEnabled) {
            return;
        }
        Vector2 mouseInput = InputManager.Instance.GetMouseVector();

        float mouseSens = mouseSensivity * PlayerPrefs.GetFloat(PlayerPrefsConstants.MOUSE_SENSITIVITY, 1);

        float mouseX = mouseInput.x * Time.smoothDeltaTime * mouseSens;
        float mouseY = mouseInput.y * Time.smoothDeltaTime * mouseSens;

        if (flashlightTransform != null && Flashlight.Instance.isActive) {
            flashlightRotation.x -= mouseY / flashlightRotSensFactor;
            flashlightRotation.y += mouseX / flashlightRotSensFactor;

            flashlightRotation.x = Mathf.Clamp(flashlightRotation.x, -flashlightRotationMax, flashlightRotationMax);
            flashlightRotation.y = Mathf.Clamp(flashlightRotation.y, -flashlightRotationMax, flashlightRotationMax);

            flashlightTransform.localRotation = Quaternion.Euler(flashlightRotation);
        }

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -maxRotationY, maxRotationY);


        if (maxRotationX > defaultMaxRotationX) {
            rotationX += mouseX;
            rotationX = Mathf.Clamp(rotationX, -maxRotationX, maxRotationX);
            headTransform.localRotation = Quaternion.Euler(new Vector3(rotationY, rotationX, 0));
        } else {
            headTransform.localRotation = Quaternion.Euler(new Vector3(rotationY, 0, 0));
            transform.Rotate(Vector3.up, mouseX);
        }
    }
    private void HandleMovement() {
        if (!controller.enabled) {
            return;
        }
        Vector2 inputVector = InputManager.Instance.GetMovementVectorNormalized();
        Vector3 moveDirection = transform.right * inputVector.x + transform.forward * inputVector.y;

        isMoving = isGrounded && moveDirection != Vector3.zero;

        if (isSprinting && inputVector.y > 0 && !isCrouching) {
            controller.Move(moveDirection * sprintSpeed * Time.deltaTime);
        } else {
            if (isCrouching) {
                controller.Move(moveDirection * crouchSpeed * Time.deltaTime);
            } else {
                controller.Move(moveDirection * walkSpeed * Time.deltaTime);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void HandleCrouch() {
        if (isCrouching) {
            controller.height = Mathf.Max(controller.height - crouchTransitionSpeed * Time.deltaTime, crouchHeight);
        } else {
            controller.height = Mathf.Min(controller.height + crouchTransitionSpeed * Time.deltaTime, normalHeight);
            if (controller.height < normalHeight) {
                transform.position = transform.position + new Vector3(0, Mathf.Lerp(normalHeight, transform.position.y, Time.deltaTime), 0) * Time.deltaTime;
            }
        }
    }
    private void HandleInteraction() {
        bool interactHitBool = Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit interactHit, interactDistance, interactableMask);
        bool nonInteractHitBool = Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit nonInteractHit, interactDistance, nonInteractableMask);

        if (interactHitBool && !nonInteractHitBool || interactHitBool && nonInteractHitBool && interactHit.distance < nonInteractHit.distance) {
            if (interactHit.transform.TryGetComponent(out Interactable interactable)) {
                InteractionMessageUI.Instance.Show(interactable.interactMessage);
                CrosshairUI.Instance.Hover();
                if (isInteracting) {
                    interactable.InteractAction();
                }
            }
        } else {
            InteractionMessageUI.Instance.Hide();
            CrosshairUI.Instance.StopHover();
        }
        if (isInteracting) {
            isInteracting = false;
        }
    }
    // public
    public void Hide() {
        ResetMovementFlags();
        ResetHeight();
        if (Flashlight.Instance.isActive) {
            Flashlight.Instance.UnequipImmediately();
        }
        headTransform.localRotation = Quaternion.Euler(Vector3.zero);
        DisableCharacterController();
        isHiding = true;
    }
    public void UnHide() {
        EnableCharacterController();
        isHiding = false;
    }
    public void DisableCharacterController() {
        controller.enabled = false;
        isSprinting = false;
        isMoving = false;
    }
    public bool IsControllerEnabled() {
        return controller.enabled;
    }
    public void EnableCharacterController() {
        controller.enabled = true;
    }
    public void DisableCameraLook() {
        cameraLookEnabled = false;
    }
    public void EnableCameraLook() {
        cameraLookEnabled = true;
    }
    public void RestrictRotation(float maxX, float maxY) {
        maxRotationX = maxX;
        maxRotationY = maxY;
    }
    public void RestrictRotation(float numXY) {
        maxRotationX = numXY;
        maxRotationY = numXY;
    }
    public void UnrestrictRotation() {
        maxRotationX = defaultMaxRotationX;
        maxRotationY = defaultMaxRotationY;
    }
    public void ResetHeadRotation() {
        headTransform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    // private
    private void ResetHeight() {
        controller.height = normalHeight;
    }
    private void ResetMovementFlags() {
        isMoving = false;
        isSprinting = false;
        isCrouching = false;
        isLanding = false;
        isFloating = false;
    }
    float groundCheckOffset = .2f;
    private bool IsGrounded() {
        Vector3 checkPosition = new Vector3(transform.position.x, transform.position.y - controller.height / 2 + groundCheckRadius - groundCheckOffset, transform.position.z);
        return Physics.CheckSphere(checkPosition, groundCheckRadius, groundMask);
    }
    private void CheckIsLandingAndIsFloating() {
        if (isLanding) {
            isLanding = false;
        }
        if (isGrounded && isFloating) {
            isLanding = true;
        }
        if (!isGrounded) {
            isFloating = true;
        } else {
            isFloating = false;
        }

    }
    // events
    private void OnCrouch(object sender, System.EventArgs e) {
        if (canCrouch) {
            isCrouching = !isCrouching;
        }
    }
    private void OnSprintStarted(object sender, System.EventArgs e) {
        if (canSprint) {
            isSprinting = true;
        }
    }
    private void OnSprintCanceled(object sender, System.EventArgs e) {
        isSprinting = false;
    }
    private void OnInteract(object sender, System.EventArgs e) {
        isInteracting = true;
    }
    // public setters
    public void SetInChase(bool inChase) {
        this.inChase = inChase;
    }

}
