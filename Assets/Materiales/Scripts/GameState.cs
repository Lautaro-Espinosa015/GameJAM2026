using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState I; 

    public string alineacion = "neutral";
    public bool eligioAyudarAJuan = false;
    public int reputacionConGremio = 0;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }
}
