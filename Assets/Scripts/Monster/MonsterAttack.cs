using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {
    [SerializeField] private MonsterAnimation animationController;
    [SerializeField] private MonsterSounds sounds;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackInterval = 0.9f;
    [SerializeField] private float damage = 70f;

    private bool canAttack = true;

    private void Update() {
        if (PlayerUtils.DistanceToPlayer(transform.position) <= attackDistance && canAttack && !PlayerController.Instance.isHiding) {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack() {
        canAttack = false;
        sounds.PlayAttackSound();
        yield return new WaitForSeconds(0.1f);
        PlayerHealth.Instance.TakeDamage(damage);
        animationController.Attack();
        yield return new WaitForSeconds(attackInterval);

        canAttack = true;
    }
}
