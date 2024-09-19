using UnityEngine;

public class MonsterSounds : MonoBehaviour {
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] randomRoars;
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private float footstepWalkTimerMax = .6f;
    [SerializeField] private float footstepRunTimerMax = .3f;
    [SerializeField] private float footstepVolume = 0.09f;
    [SerializeField] private float roarVolume = 0.4f;
    [SerializeField] private float attackVolume = 0.3f;
    private float roarIntervalMin = 5f;
    private float roarIntervalMax = 25f;
    private float timeUntilNextRoar;
    private float roarTimer;
    private float footstepTimer;

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
        SoundManager.Instance.PlaySound(attackSounds, transform.position, attackVolume);
    }
    private void PlayFootstep(float timerMax) {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0) {
            footstepTimer = timerMax;
            SoundManager.Instance.PlaySound(footstepSounds, transform.position, footstepVolume, maxDistance: 15f, rolloffMode: AudioRolloffMode.Custom);
        }
    }
}
