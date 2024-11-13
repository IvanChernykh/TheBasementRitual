using UnityEngine;

public class MonsterSounds : MonoBehaviour {
    [Header("Sounds")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] randomRoars;
    [SerializeField] private AudioClip attackSound;

    [Header("Footstep Timer")]
    [SerializeField] private float footstepWalkTimerMax = .6f;
    [SerializeField] private float footstepRunTimerMax = .3f;

    [Header("Volume")]
    [SerializeField] private float footstepVolume = 0.1f;
    [SerializeField] private float attackVolume = 0.3f;
    private float roarVolume = 0.25f;
    private readonly float roarIntervalMin = 6f;
    private readonly float roarIntervalMax = 30f;
    private float timeUntilNextRoar;
    private float roarTimer;
    private float footstepTimer;

    private void Start() {
        timeUntilNextRoar = Random.Range(roarIntervalMin, roarIntervalMax);
    }
    public void PlayRandomRoar() {
        roarTimer += Time.deltaTime;
        if (roarTimer >= timeUntilNextRoar) {
            PlayRandomRoarImmediately();
        }
    }
    public void PlayRandomRoarImmediately(float maxDistance = 20f) {
        SoundManager.Instance.PlaySound(randomRoars, transform.position, roarVolume, maxDistance: maxDistance, rolloffMode: AudioRolloffMode.Custom);
        timeUntilNextRoar = Random.Range(roarIntervalMin, roarIntervalMax);
        roarTimer = 0f;
    }
    public void PlayRunFootstep() {
        PlayFootstep(footstepRunTimerMax);
    }
    public void PlayWalkFootstep() {
        PlayFootstep(footstepWalkTimerMax);
    }
    public void PlayAttackSound() {
        SoundManager.Instance.PlaySound(attackSound, transform.position, attackVolume);
    }
    private void PlayFootstep(float timerMax) {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0) {
            footstepTimer = timerMax;
            SoundManager.Instance.PlaySound(footstepSounds, transform.position, footstepVolume, maxDistance: 20f, rolloffMode: AudioRolloffMode.Custom);
        }
    }
}
