using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Jugar ()
    {
        SceneManager.LoadScene(1);
    }

    public void Salir()
    {
        Debug.Log("saliendo...");
        Application.Quit();
    }
}
