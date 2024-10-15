using UnityEngine;

public class Safe_1 : MonoBehaviour {
    [SerializeField] private string[] comboVariants;
    [SerializeField] private KeypadSimple keypad;

    private void Start() {
        keypad.SetCombo(comboVariants[Random.Range(0, comboVariants.Length)]);
    }
}
