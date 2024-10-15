using UnityEngine;
using UnityEngine.Events;

public class KeypadSimple : KeypadBase {
    [Header("Events")]
    [SerializeField] private UnityEvent onOpen;

    [Header("Combination")]
    [SerializeField] private string keypadCombo = "000";

    [Header("Sound")]
    [SerializeField] private AudioClip buttonClickedSfx;

    private bool opened;

    public override void AddInput(string input) {
        SoundManager.Instance.PlaySound(buttonClickedSfx, transform.position, volume: .5f, minDistance: .5f);
        if (opened) {
            return;
        }
        currentInput += input;

        if (currentInput.Length > keypadCombo.Length) {
            currentInput = currentInput.Substring(currentInput.Length - keypadCombo.Length);
        }
        if (currentInput == keypadCombo) {
            opened = true;
            onOpen?.Invoke();
        }
    }

    public void SetCombo(string newCombo) {
        keypadCombo = newCombo;
    }
}
