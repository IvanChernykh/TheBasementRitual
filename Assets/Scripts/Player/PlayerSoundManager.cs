using UnityEngine;

public class PlayerSoundManager : MonoBehaviour {
    public static PlayerSoundManager Instance;
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField] private AudioClip[] sprintSounds;
    [SerializeField] private AudioClip[] jumpStartSounds;
    [SerializeField] private AudioClip[] landingSounds;
    private PlayerController player;
    private float footstepWalkTimer;
    private float footstepRunTimer;
    private float footstepWalkTimerMax = .6f;
    private float footStepRunTimerMax = .3f;
    private Vector3 footSoundsPosition;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        player = PlayerController.Instance;
    }
    private void Update() {
        footSoundsPosition = player.gameObject.transform.position + new Vector3(0f, -0.5f, 0f);
        PlayWalkSound();
        PlaySprintSound();
        PlayLandingSound();
    }

    private void PlayWalkSound() {
        footstepWalkTimer -= Time.deltaTime;
        if (footstepWalkTimer <= 0) {
            footstepWalkTimer = footstepWalkTimerMax;
            if (player.isMoving && !player.isSprinting) {
                SoundManager.Instance.PlaySound(walkSounds, footSoundsPosition);
            }
        }
    }
    private void PlaySprintSound() {
        footstepRunTimer -= Time.deltaTime;
        if (footstepRunTimer <= 0) {
            footstepRunTimer = footStepRunTimerMax;
            if (player.isMoving && player.isSprinting) {
                SoundManager.Instance.PlaySound(sprintSounds, footSoundsPosition);
            }
        }
    }
    public void PlayJumpStartSound() {
        SoundManager.Instance.PlaySound(jumpStartSounds, footSoundsPosition);
    }
    private void PlayLandingSound() {
        if (player.isLanding && !player.isCrouching) {
            SoundManager.Instance.PlaySound(landingSounds, footSoundsPosition, .5f);
        }
    }
}
