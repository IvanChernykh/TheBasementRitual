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
    [SerializeField] private Vector3 crouchOffset;
    private float crouchTransitionSpeed = 3f;

    [Header("Ground Checking")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private Vector3 groundCheckDefaultPos;

    [Header("Mouse")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private float mouseSensivity = 10f;
    private float rotationY;
    private float rotationX;
    private float maxRotationX = 0; // unlimited if 0
    private float maxRotationY = 85f;
    public readonly float defaultMaxRotationY = 85f;
    public readonly float defaultMaxRotationX = 0f;

    [Header("Jumping and Gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.8f * 2;
    private Vector3 velocity = Vector3.zero;

    [Header("Interactions")]
    [SerializeField] private LayerMask interactableLayerMask;
    public float interactDistance { get; private set; } = 2f;

    // flags
    public bool isMoving { get; private set; }
    public bool isSprinting { get; private set; }
    public bool isLanding { get; private set; }
    public bool isCrouching { get; private set; }
    public bool isHiding { get; private set; }
    public bool isInChase { get; private set; }
    private bool isGrounded;
    private bool isFloating;
    private bool isJumping;
    private bool isInteracting;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
        controller = GetComponent<CharacterController>();
    }
    private void Start() {
        InputManager.Instance.OnSprintStartedEvent += OnSprintStarted;
        InputManager.Instance.OnSprintCanceledEvent += OnSprintCanceled;
        InputManager.Instance.OnCrouchEvent += OnCrouch;
        InputManager.Instance.OnJumpEvent += OnJump;
        InputManager.Instance.OnInteractEvent += OnInteract;

        groundCheckDefaultPos = groundCheck.localPosition;
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
        Vector2 mouseInput = InputManager.Instance.GetMouseVector();

        float mouseX = mouseInput.x * Time.smoothDeltaTime * mouseSensivity;
        float mouseY = mouseInput.y * Time.smoothDeltaTime * mouseSensivity;

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

        if (isJumping && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isJumping = false;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void HandleCrouch() {
        if (isCrouching) {
            controller.height = controller.height - crouchTransitionSpeed * Time.deltaTime;
            if (controller.height <= crouchHeight) {
                controller.height = crouchHeight;
            } else {
                transform.position = transform.position - crouchOffset * Time.deltaTime;
            }
        } else {
            controller.height = controller.height + crouchTransitionSpeed * Time.deltaTime;
            if (controller.height < normalHeight) {
                transform.position = transform.position + new Vector3(0, Mathf.Lerp(crouchOffset.y, transform.position.y, Time.deltaTime), 0) * Time.deltaTime;
            }
            if (controller.height >= normalHeight) {
                controller.height = normalHeight;
            }
        }
    }
    private void HandleInteraction() {
        if (Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit raycastHit, interactDistance, interactableLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out Interactable interactable)) {
                InteractionMessageUI.Instance.Show(interactable.interactMessage);
                CrosshairUI.Instance.Hover();
                if (isInteracting) {
                    interactable.InteractAction();
                }
            }
        } else {
            InteractionMessageUI.Instance.Hide();
            CrosshairUI.Instance.Hide();
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
        controller.enabled = false;
        isHiding = true;
    }
    public void UnHide() {
        controller.enabled = true;
        isHiding = false;
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
    // private
    private void ResetHeight() {
        transform.position = transform.position + crouchOffset;
        controller.height = normalHeight;
    }
    private void ResetMovementFlags() {
        isMoving = false;
        isSprinting = false;
        isCrouching = false;
        isJumping = false;
        isLanding = false;
        isFloating = false;
    }
    private bool IsGrounded() {
        if (isCrouching) {
            groundCheck.localPosition = new Vector3(0, -groundCheckRadius / 2, 0);
        } else {
            groundCheck.localPosition = groundCheckDefaultPos;
        }
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
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
        if (isGrounded) {
            isCrouching = !isCrouching;
        }
    }
    private void OnJump(object sender, System.EventArgs e) {
        if (isGrounded) {
            isJumping = true;
            PlayerSounds.Instance.PlayJumpStartSound();
        }
    }
    private void OnSprintStarted(object sender, System.EventArgs e) {
        if (isGrounded) {
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
    public void SetIsInChase(bool inChase) {
        isInChase = inChase;
    }
    // debug
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
