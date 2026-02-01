using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscenaPorProximidad2D : MonoBehaviour
{
    [Header("Opcional: Mostrar mensaje en consola/UI")]
    public bool debugMensajes = true;

    private bool jugadorEnRango = false;

    // Asegurate de que este objeto tenga un Collider2D con "Is Trigger" activado.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            if (debugMensajes) Debug.Log("Estás cerca. Presioná F para cambiar de escena.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            if (debugMensajes) Debug.Log("Te alejaste del objeto.");
        }
    }

    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.F))
        {
            // Podés cargar por índice o por nombre:
             SceneManager.LoadScene(2);
            
        }
    }
}


