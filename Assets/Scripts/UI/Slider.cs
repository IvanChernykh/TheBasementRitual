using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private UnityEvent onSliderChangeEvent;

    public float Value { get; private set; }

    void Start() {
        slider.onValueChanged.AddListener((v) => {
            Value = v;
            onSliderChangeEvent?.Invoke();
        });
    }
}
