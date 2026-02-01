using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogoBasico : MonoBehaviour
{
    [Header("UI Diálogo")]
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;

    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Audio (opcional)")]
    [SerializeField] private AudioSource footstepAudio;

    [Header("Comportamiento al finalizar")]
    [Tooltip("Si está activo, al terminar el último diálogo se carga la escena del menú.")]
    [SerializeField] private bool goToMenuOnFinish = true;

    [Tooltip("Nombre de la escena del menú (debe estar en Build Settings).")]
    [SerializeField] private string menuSceneName = "MainMenu";

    [Tooltip("Si es mayor a 0, espera este tiempo antes de cambiar a la escena del menú.")]
    [SerializeField] private float returnDelay = 0.0f;

    // Ajustes internos
    private float typingTime = 0.05f;
    private bool didDialogueStart;
    private bool isPlayerInRange;
    private int lineIndex;

    private void Awake()
    {
        // Asegurar referencia al Player si no fue asignada en el Inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.GetComponent<Player>();
            }
        }

        // Opcional: si no se asignó footstepAudio por Inspector, intentar tomar uno del mismo GameObject
        if (footstepAudio == null)
        {
            footstepAudio = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(true);
        lineIndex = 0;

        if (player != null)
        {
            player.DisableMovement();
        }

        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;

        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            // Fin del diálogo
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            if (player != null) player.EnableMovement();
            dialogueMark.SetActive(isPlayerInRange);

            // --- Aquí va el retorno al menú ---
            if (goToMenuOnFinish)
            {
                if (returnDelay > 0f)
                {
                    // Opción opcional: esperar un momento antes de cambiar de escena
                    StartCoroutine(ReturnToMenuAfterDelay(returnDelay));
                }
                else
                {
                    // Cambio directo a la escena del menú
                    LoadMenuScene();
                }
            }
        }
    }

    private IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadMenuScene();
    }

    private void LoadMenuScene()
    {
        
        SceneManager.LoadScene(0);
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
            Debug.Log("Se puede hablar");

            if (footstepAudio != null)
            {
                footstepAudio.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
            Debug.Log("No se puede hablar");
        }
    }
}
