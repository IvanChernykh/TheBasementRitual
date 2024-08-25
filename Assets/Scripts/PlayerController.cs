using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    private CharacterController controller;
    private float normalHeight = 1.8f;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float crouchSpeed = 3f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight;
    [SerializeField] private Vector3 crouchOffset;

    [Header("Ground Checking")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private Vector3 groundCheckDefaultPos;

    [Header("Mouse")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensivity = 10f;
    private float rotationY;

    [Header("Jumping and Gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.8f * 2;
    private Vector3 velocity = Vector3.zero;

    [Header("Interactions")]
    [SerializeField] private LayerMask interactableLayerMask;
    private bool isGrounded;
    public bool isMoving { get; private set; }
    public bool isSprinting { get; private set; }
    public bool isLanding { get; private set; }
    private bool isFloating;
    private bool isJumping;
    private bool isInteracting;
    private bool isCrouching;

    private void Awake() {
        Instance = this;
        controller = GetComponent<CharacterController>();
    }
    private void Start() {
        InputManager.Instance.OnSprintStartedEvent += OnSprintStarted;
        InputManager.Instance.OnSprintCanceledEvent += OnSprintCanceled;
        InputManager.Instance.OnCrouchEvent += OnCrouch;
        InputManager.Instance.OnJumpEvent += OnJump;
        InputManager.Instance.OnInteractEvent += OnInteract;

        Cursor.lockState = CursorLockMode.Locked;
        groundCheckDefaultPos = groundCheck.localPosition;
    }
    private void Update() {
        isGrounded = IsGrounded();

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2;
        }
        HandleMovement();
        HandleCrouch();
        HandleInteraction();
        CheckIsLandingAndIsFloating();
    }
    private void LateUpdate() {
        HandleCamera();
    }
    private void HandleCamera() {
        Vector2 mouseInput = InputManager.Instance.GetMouseVector();

        float mouseX = mouseInput.x * Time.smoothDeltaTime * mouseSensivity;
        float mouseY = mouseInput.y * Time.smoothDeltaTime * mouseSensivity;

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -85f, 85f);

        cameraTransform.localRotation = Quaternion.Euler(new Vector3(rotationY, 0, 0));
        transform.Rotate(Vector3.up, mouseX);
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
            controller.height = controller.height - crouchSpeed * Time.deltaTime;
            if (controller.height <= crouchHeight) {
                controller.height = crouchHeight;
            } else {
                transform.position = transform.position - crouchOffset * Time.deltaTime;
            }
        } else {
            controller.height = controller.height + crouchSpeed * Time.deltaTime;
            if (controller.height < normalHeight) {
                transform.position = transform.position + new Vector3(0, Mathf.Lerp(crouchOffset.y, transform.position.y, Time.deltaTime), 0) * Time.deltaTime;
            }
            if (controller.height >= normalHeight) {
                controller.height = normalHeight;
            }
        }
    }
    private bool IsGrounded() {
        if (isCrouching) {
            groundCheck.localPosition = new Vector3(0, -groundCheckRadius / 2, 0);
        } else {
            groundCheck.localPosition = groundCheckDefaultPos;
        }
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }
    private void HandleInteraction() {
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactDistance, interactableLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out Door interactable) && isInteracting) {
                Debug.Log("interact 2");
                interactable.Interact();
            }
        }
        if (isInteracting) {
            isInteracting = false;
        }
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
            PlayerSoundManager.Instance.PlayJumpStartSound();
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
    // debug
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
