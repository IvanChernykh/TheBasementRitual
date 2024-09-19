using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private float initialHealth = 100f;
    [SerializeField] private float restoreSpeed = 10f;
    [SerializeField] private float restoreDelay = 1f;

    private float currentHealth;
    private bool isDead = false;

    private void Awake() {
        if (Instance != null) {
            Exceptions.MoreThanOneInstance(name);
        }
        Instance = this;
    }
    private void Start() {
        currentHealth = initialHealth;
    }

    public void TakeDamage(float damage) {
        if (isDead) {
            return;
        }
        currentHealth -= damage;
        DamageOverlay.Instance.Show(GetTimeToRestoreHealth(), restoreDelay);
        PlayerSounds.Instance.PlayTakeDamageVoice();
        if (currentHealth <= 0) {
            Die();
            return;
        }
        StopAllCoroutines();
        StartCoroutine(RestoreHealthAfterDelay());
    }
    public void Kill() {
        Die();
    }
    private void Die() {
        isDead = true;
        GameStateManager.Instance.EnterGameOverState();
    }
    private float GetTimeToRestoreHealth() {
        float missingHealthPercent = (initialHealth - currentHealth) / initialHealth;
        return missingHealthPercent * (initialHealth / restoreSpeed);
    }
    private IEnumerator RestoreHealthAfterDelay() {
        yield return new WaitForSeconds(restoreDelay);

        while (currentHealth < initialHealth) {
            currentHealth += restoreSpeed * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, initialHealth);
            yield return null;
        }
    }
}
