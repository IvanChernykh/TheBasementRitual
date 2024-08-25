using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float normalHeight = 1.8f;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float crouchSpeed = 3f;
    [Header("Crouch")]
    [SerializeField] private float crouchHeight;
    [SerializeField] private Vector3 crouchOffset;
    [Header("Ground Checking")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundMask;
    private Vector3 groundCheckDefaultPos;

    [Header("Mouse")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensivity = 10f;
    private float rotationY;
    public float jumpHeight = 1.5f;
    private CharacterController controller;
    private float gravity = -9.8f;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded;
    private bool isWalking;
    private bool isCrouching;
    private float footstepTimer;
    private float footstepTimerMax = .5f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        groundCheckDefaultPos = groundCheck.localPosition;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    private void Update() {
        calculateGroundCheckPos();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = 0;
        }
        HandleMovement();
        crouch();
        // footstepTimer -= Time.deltaTime;
        // if (footstepTimer <= 0) {
        //     footstepTimer = footstepTimerMax;
        //     if (isWalking) {
        //         // PlayerAudio.Instance.PlayWalkSound(transform.position);
        //     }
        // }
    }
    private void LateUpdate() {
        MoveCamera();
    }
    private void MoveCamera() {
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

        isWalking = moveDirection != Vector3.zero;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void crouch() {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded) {
            isCrouching = !isCrouching;
        }
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
    void calculateGroundCheckPos() {
        if (isCrouching) {
            groundCheck.localPosition = new Vector3(0, -groundCheckRadius / 2, 0);
        } else {
            groundCheck.localPosition = groundCheckDefaultPos;
        }
    }
}
