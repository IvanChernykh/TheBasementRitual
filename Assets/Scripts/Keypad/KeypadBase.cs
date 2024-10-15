using UnityEngine;

public abstract class KeypadBase : MonoBehaviour {
    protected string currentInput;

    public abstract void AddInput(string input);
}
