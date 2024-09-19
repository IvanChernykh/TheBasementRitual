using UnityEngine;
using Assets.Scripts.Utils;

public class PlayerSounds : MonoBehaviour {
    public static PlayerSounds Instance { get; private set; }
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField] private AudioClip[] sprintSounds;
    [SerializeField] private AudioClip[] jumpStartSounds;
    [SerializeField] private AudioClip[] landingSounds;
    [SerializeField] private AudioClip[] flashlightOnSounds;
    [SerializeField] private AudioClip[] voiceTakeDamage;
    private PlayerController player;
    private float footstepWalkTimer;
    private float footstepRunTimer;
    private readonly float footstepWalkTimerMax = .6f;
    private readonly float footStepRunTimerMax = .3f;
    // volume
    private readonly float footstepVolume = .2f;
    private readonly float landingVolume = .1f;
    private readonly float flashlightVolume = .1f;
    private readonly float takeDamageVoiceVolume = .25f;
    private Vector3 footSoundsPosition;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        player = PlayerController.Instance;
    }
    private void Update() {
        footSoundsPosition = player.transform.position + new Vector3(0f, -0.5f, 0f);
        PlayWalkSound();
        PlaySprintSound();
        PlayLandingSound();
    }

    private void PlayWalkSound() {
        footstepWalkTimer -= Time.deltaTime;
        if (footstepWalkTimer <= 0) {
            footstepWalkTimer = footstepWalkTimerMax;
            if (player.isMoving && !player.isSprinting) {
                SoundManager.Instance.PlaySound2D(walkSounds, footSoundsPosition, footstepVolume);
            }
        }
    }
    private void PlaySprintSound() {
        footstepRunTimer -= Time.deltaTime;
        if (footstepRunTimer <= 0) {
            footstepRunTimer = footStepRunTimerMax;
            if (player.isMoving && player.isSprinting) {
                SoundManager.Instance.PlaySound2D(sprintSounds, footSoundsPosition, footstepVolume);
            }
        }
    }
    public void PlayJumpStartSound() {
        SoundManager.Instance.PlaySound2D(jumpStartSounds, footSoundsPosition, footstepVolume);
    }
    private void PlayLandingSound() {
        if (player.isLanding && !player.isCrouching) {
            SoundManager.Instance.PlaySound2D(landingSounds, footSoundsPosition, landingVolume);
        }
    }
    public void PlayFlashlightOnSound() {
        SoundManager.Instance.PlaySound2D(flashlightOnSounds, transform.position, flashlightVolume);
    }
    public void PlayTakeDamageVoice() {
        SoundManager.Instance.PlaySound2D(voiceTakeDamage, transform.position, takeDamageVoiceVolume);
    }
}
