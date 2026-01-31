using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject BotonPausa;
    public GameObject PanelPausa;

    private void Awake()
    {
       
        Time.timeScale = 1f;
        if (PanelPausa != null) PanelPausa.SetActive(false);
        if (BotonPausa != null) BotonPausa.SetActive(true);

        
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        PanelPausa.SetActive(true);
        BotonPausa.SetActive(false);
     
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PanelPausa.SetActive(false);
        BotonPausa.SetActive(true);
    
    }

    public void RestardGame()
    {
      
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(0);
    }
}
