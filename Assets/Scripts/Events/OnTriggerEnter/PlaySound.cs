using UnityEngine;

public class PlaySound : OnTriggerEnterBase {
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private Vector3 positionToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;

    protected override void HandleEvent() {
        SoundManager.Instance.PlaySound(audioClips, positionToPlay, volume, minDistance, maxDistance);
    }
}
