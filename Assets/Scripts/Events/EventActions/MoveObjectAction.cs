using System.Collections;
using UnityEngine;

public class MoveObjectAction : EventAction {
    [Header("Action Settings")]
    [SerializeField] private float delay;
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveTime = 2f;
    [SerializeField] private bool moveInCycle;
    [Tooltip("Unlimited if 0"), SerializeField] private float cycleDuration = 0f;

    private bool movingToB = true;

    public override void ExecuteAction() {
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject() {
        if (delay > 0) {
            yield return new WaitForSeconds(delay);
        }
        float cycleElapsedTime = 0f;

        while (true) {
            Vector3 startPosition = movingToB ? pointA.position : pointB.position;
            Vector3 targetPosition = movingToB ? pointB.position : pointA.position;
            float elapsedTime = 0f;

            while (elapsedTime < moveTime) {
                objectToMove.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                cycleElapsedTime += Time.deltaTime;

                if (moveInCycle && cycleDuration > 0 && cycleElapsedTime >= cycleDuration) {
                    yield break;
                }
                yield return null;
            }

            objectToMove.position = targetPosition;

            if (!moveInCycle) {
                break;
            }

            movingToB = !movingToB;
            if (cycleDuration > 0 && cycleElapsedTime >= cycleDuration) {
                yield break;
            }

            yield return null;
        }
    }
}
