using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/NoteData")]
public class NoteData : ScriptableObject {
    public string noteName;
    [TextArea(10, 10)]
    public string noteText;
    public string localizationKey;
}
