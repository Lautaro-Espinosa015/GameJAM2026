
using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    [TextArea(1, 2)] public string texto;
    public int siguienteLinea = -1;   // -1 = terminar diálogo
    public string efecto;             // p.ej: "alineacion=heroico;reputacion+=5;ayudarJuan=true"
}

