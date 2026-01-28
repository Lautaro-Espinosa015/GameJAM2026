using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(4, 6)] public string texto;
    public DialogueOption[] opciones; // null o vacío = continúa con F como tu flujo actual
}
