using UnityEngine;

public class DeepImpactsZone : MonoBehaviour {
    [Header("Audio")]
    [SerializeField] private AudioSource deepImpacts;
    [SerializeField] private AudioSource deepImpactsStress;

    [Header("Settings")]
    [SerializeField] private bool stressZone;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private bool deactivateOnExit;

    [Header("Conditions")]
    [SerializeField] private EventCondition[] conditions;
    private bool isActive = true;

    private void OnTriggerStay(Collider other) {
        if (isActive) {
            bool conditionMet = CheckConditions();
            if (conditionMet && other.CompareTag("Player")) {
                HandlePlay();
                HandleStop();
            }
        }
    }
    private void OnTriggerExit() {
        Stop();
        if (deactivateOnExit) {
            isActive = false;
        }
    }
    private void HandlePlay() {
        if (BackgroundMusic.Instance.CanPlayDeepImpacts()) {
            if (stressZone) {
                if (!deepImpactsStress.isPlaying) {
                    BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.DeepImpactsStress, fadeInTime);
                }
            } else {
                if (!deepImpacts.isPlaying) {
                    BackgroundMusic.Instance.Play(BackgroundMusic.Sounds.DeepImpacts, fadeInTime);
                }
            }
        }
    }
    private void HandleStop() {
        if (!BackgroundMusic.Instance.CanPlayDeepImpacts()) {
            Stop();
        }
    }
    private void Stop() {
        if (stressZone) {
            BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.DeepImpactsStress, fadeOutTime);
        } else {
            BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.DeepImpacts, fadeOutTime);
        }
    }
    private bool CheckConditions() {
        bool conditionMet = true;
        foreach (EventCondition condition in conditions) {
            if (!condition.IsConditionMet()) {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}
