using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {
    [SerializeField] private MonsterCore monster;
    [SerializeField] private float attackDistance = 1f;

    private bool canAttack = true;

    private void Update() {
        if (PlayerUtils.DistanceToPlayer(transform.position) <= attackDistance && canAttack && !PlayerController.Instance.isHiding) {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack() {
        monster.Animation.Attack();
        yield return new WaitForSeconds(0.2f);
        PlayerHealth.Instance.Kill();
    }
}
