using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class UnconScreen : MonoBehaviour {
    [SerializeField] private Image background;

    private readonly float unconDelay = 3f;
    private readonly float unconFadeTime = 3f;

    public void WakeUp() {
        StartCoroutine(WakeUpCoroutine());
    }

    private IEnumerator WakeUpCoroutine() {
        PlayerController.Instance.DisableCameraLook();
        PlayerController.Instance.DisableCharacterController();
        yield return new WaitForSeconds(unconDelay);
        PlayerController.Instance.EnableCharacterController();
        PlayerController.Instance.EnableCameraLook();
        yield return StartCoroutine(UI.FadeGraphic(background, unconFadeTime, fadeIn: false));
        Destroy(gameObject);
    }
}
