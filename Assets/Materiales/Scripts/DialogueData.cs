using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogues/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}
