using UnityEngine;

public class CameraShaking : MonoBehaviour {
    [SerializeField] private float walkShakeSpeed = 5f;
    [SerializeField] private float sprintShakeSpeed = 10f;
    [SerializeField] private float maxShakeAmountX = 0.02f;
    [SerializeField] private float maxShakeAmountY = 0.02f;
    [SerializeField] private float randomMultiplier = 0.005f;
    [SerializeField] private float shakeSmoothSpeed = 2f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private float returnThreshold = 0.001f;
    private float shakeSpeed;
    private float defaultYPos;
    private float defaultXPos;
    private float currentShakeAmountX = 0f;
    private float currentShakeAmountY = 0f;
    private float timer = 0;


    private void Start() {
        defaultYPos = transform.localPosition.y;
        defaultXPos = transform.localPosition.x;
    }
    private void Update() {
        if (PlayerController.Instance.isMoving) {
            shakeSpeed = walkShakeSpeed;
        }
        if (PlayerController.Instance.isSprinting) {
            shakeSpeed = sprintShakeSpeed;
        }
        HandleCameraShake();
    }

    private void HandleCameraShake() {
        if (PlayerController.Instance.isMoving) {
            float maxShakeAmtX = maxShakeAmountX + Random.Range(-randomMultiplier, randomMultiplier);
            float maxShakeAmtY = maxShakeAmountY + Random.Range(-randomMultiplier, randomMultiplier);

            currentShakeAmountX = Mathf.Lerp(currentShakeAmountX, maxShakeAmtX, Time.deltaTime * shakeSmoothSpeed);
            currentShakeAmountY = Mathf.Lerp(currentShakeAmountY, maxShakeAmtY, Time.deltaTime * shakeSmoothSpeed);

            timer += Time.deltaTime * (shakeSpeed + Random.Range(-1, 1));

            float newYPos = defaultYPos + Mathf.Sin(timer * 2) * currentShakeAmountY;
            float newXPos = defaultXPos + Mathf.Cos(timer) * currentShakeAmountX;

            transform.localPosition = new Vector3(newXPos, newYPos, transform.localPosition.z);
        } else {
            timer = 0;

            currentShakeAmountX = Mathf.Lerp(currentShakeAmountX, 0f, Time.deltaTime * shakeSmoothSpeed);
            currentShakeAmountY = Mathf.Lerp(currentShakeAmountY, 0f, Time.deltaTime * shakeSmoothSpeed);

            float smoothYPos = Mathf.Lerp(transform.localPosition.y, defaultYPos, Time.deltaTime * returnSpeed);
            float smoothXPos = Mathf.Lerp(transform.localPosition.x, defaultXPos, Time.deltaTime * returnSpeed);

            if (Mathf.Abs(smoothXPos - defaultXPos) < returnThreshold && Mathf.Abs(smoothYPos - defaultYPos) < returnThreshold) {
                smoothXPos = defaultXPos;
                smoothYPos = defaultYPos;
            }
            transform.localPosition = new Vector3(smoothXPos, smoothYPos, transform.localPosition.z);
        }
    }
}
