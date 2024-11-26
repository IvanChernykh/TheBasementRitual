using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {
    [SerializeField] private MonsterCore monster;
    [SerializeField] private float attackDistance = 1.6f;
    [SerializeField] private bool showEndGameOnKillPlayer = false;

    public float AttackDistance { get => attackDistance; }
    private bool isAttacking;

    private void Update() {
        if (!isAttacking && !PlayerController.Instance.isHiding && PlayerUtils.DistanceToPlayer(transform.position) <= attackDistance) {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack() {
        BackgroundMusic.Instance.Stop(BackgroundMusic.Sounds.ChaseMusic, .8f);
        BackgroundMusic.Instance.StopDeepImpacts(fadeTime: .8f);
        Vector3 monsterPos = transform.position;
        Vector3 playerPos = PlayerController.Instance.transform.position;

        PlayerController.Instance.DisableCharacterController();
        PlayerController.Instance.DisableCameraLook();
        PlayerController.Instance.ResetHeadRotation();

        if (Flashlight.Instance.isActive) {
            Flashlight.Instance.UnEquip();
            BatteryUI.Instance.Hide();
        }

        monster.Sounds.PlayAttackSound();

        transform.LookAt(new Vector3(playerPos.x, monsterPos.y, playerPos.z));
        PlayerController.Instance.transform.LookAt(new Vector3(monsterPos.x, playerPos.y, monsterPos.z));

        monster.Animation.Attack();
        yield return new WaitForSeconds(.9f);
        PlayerHealth.Instance.Kill(showEndGameOnKillPlayer);
    }
}
