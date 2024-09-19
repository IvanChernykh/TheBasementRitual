using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private float damage = 70f;

    private bool canAttack = true;

    private void Update() {
        if (PlayerUtils.DistanceToPlayer(transform.position) <= attackDistance && canAttack && !PlayerController.Instance.isHiding) {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack() {
        canAttack = false;
        PlayerHealth.Instance.TakeDamage(damage);

        yield return new WaitForSeconds(attackInterval);

        canAttack = true;
    }
}
