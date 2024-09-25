using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {
    [SerializeField] private MonsterCore monster;
    [SerializeField] private float attackDistance = 1.6f;

    public float AttackDistance { get => attackDistance; }
    private void Update() {
        try {
            if (PlayerUtils.DistanceToPlayer(transform.position) <= attackDistance && !PlayerController.Instance.isHiding) {
                StartCoroutine(Attack());
            }
        }
        catch (System.Exception e) {
            Debug.LogError("Attack error: " + e.Message);
        }
    }
    private IEnumerator Attack() {
        monster.Animation.Attack();
        yield return new WaitForSeconds(0.2f);
        PlayerHealth.Instance.Kill();
    }
}
