using UnityEngine;
using System.Collections;
using TMPro;
public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;

    private float typingTime = 0.05f;
    private bool didDialogueStart;
    private bool isPlayerInRange;
    private int lineIndex;
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach(char ch in dialogueLines[lineIndex])
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
        StartCoroutine(ShowLine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
            Debug.Log("Se puede hablar");
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
