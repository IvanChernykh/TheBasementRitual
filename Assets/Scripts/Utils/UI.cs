using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils {
    public class UI : MonoBehaviour {
        public static IEnumerator FadeGraphic(Graphic graphic, float duration, bool fadeIn) {
            float elapsedTime = 0f;
            Color color = graphic.color;
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = fadeIn ? 1f : 0f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                graphic.color = color;
                yield return null;
            }

            color.a = endAlpha;
            graphic.color = color;
        }
    }
}
