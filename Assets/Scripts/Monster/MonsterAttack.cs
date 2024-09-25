using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {
    [SerializeField] private MonsterCore monster;
    [SerializeField] private float attackDistance = 1.6f;

    public float AttackDistance { get => attackDistance; }
    private bool isAttacking;
    private void Update() {
        try {
            if (!isAttacking && !PlayerController.Instance.isHiding && PlayerUtils.DistanceToPlayer(transform.position) <= attackDistance) {
                StartCoroutine(Attack());
            }
        }
        catch (System.Exception e) {
            Debug.LogError("Attack error: " + e.Message);
        }
    }
    private IEnumerator Attack() {
        isAttacking = true;
        monster.Animation.Attack();
        yield return new WaitForSeconds(0.2f);
        PlayerHealth.Instance.Kill();
        isAttacking = false;
    }
}
