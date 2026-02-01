using TMPro; // Solo si usas TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuGanar;
    private int piezasListas = 0;

    public void ContarPieza()
    {
        piezasListas++;
        Debug.Log("Pieza encajada. Total: " + piezasListas); // Esto aparecerá en la Consola

        if (piezasListas >= 8)
        {
            if (menuGanar != null)
            {
                menuGanar.SetActive(true);
                Debug.Log("¡Juego completado! Activando menú.");
                SceneManager.LoadScene(3);
            }
            else
            {
                Debug.LogError("¡Error! No has arrastrado el objeto MenuGanar al GameManager en el Inspector.");
            }
        }
    }
}