using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscenaPorProximidad : MonoBehaviour
{
    [Header("Opcional: Mostrar mensaje en consola/UI")]
    public bool debugMensajes = true;

    private bool jugadorEnRango = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            if (debugMensajes) Debug.Log("Estás cerca. Presioná F para cambiar de escena.");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            if (debugMensajes) Debug.Log("Te alejaste del objeto.");
            // Aquí podrías ocultar el UI
        }
    }

    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.F))
        {
          
           
            SceneManager.LoadScene(2);
        }
    }
}


