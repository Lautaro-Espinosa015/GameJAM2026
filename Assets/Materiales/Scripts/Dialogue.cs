using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI; // Para Button

public class Dialogue : MonoBehaviour
{   
    [Header("UI")]
    [SerializeField] private GameObject dialogueMark;   // Icono/indicador para presionar F
    [SerializeField] private GameObject dialoguePanel;  // Panel del diálogo
    [SerializeField] private TMP_Text dialogueText;     // Texto del diálogo

    [Header("Opciones")]
    [SerializeField] private Transform optionsContainer;      // Un contenedor con Vertical Layout Group
    [SerializeField] private Button optionButtonPrefab;       // Prefab de botón (con TextMeshProUGUI adentro)

    [Header("Datos")]
    [SerializeField] private DialogueData dialogue;           // ScriptableObject con las líneas
    private DialogueLine[] dialogueLines => dialogue != null ? dialogue.lines : null;

    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Ajustes")]
    [SerializeField] private float typingTime = 0.05f;

    private bool didDialogueStart;
    private bool isPlayerInRange;
    private bool esperandoOpcion = false;
    private int lineIndex;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.GetComponent<Player>();
        }

        // Seguridad: ocultar panel y opciones al iniciar
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (optionsContainer != null) optionsContainer.gameObject.SetActive(false);
        if (dialogueMark != null) dialogueMark.SetActive(false);
    }

    private void Update()
    {
        if (!isPlayerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (esperandoOpcion)
            {
                // Si hay opciones visibles, ignorar F hasta que elija
                return;
            }
            else if (dialogueLines != null && lineIndex >= 0 && lineIndex < dialogueLines.Length
                     && dialogueText.text == dialogueLines[lineIndex].texto)
            {
                NextDialogueLine();
            }
            else
            {
                // Skipping el tipeo
                StopAllCoroutines();
                if (dialogueLines != null && lineIndex >= 0 && lineIndex < dialogueLines.Length)
                    dialogueText.text = dialogueLines[lineIndex].texto;
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning($"[{name}] DialogueData vacío o no asignado.");
            return;
        }

        didDialogueStart = true;
        lineIndex = 0;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (dialogueMark != null) dialogueMark.SetActive(true);

        if (player != null) player.DisableMovement();

        StartCoroutine(ShowLine());
    }

    private void EndDialogue()
    {
        didDialogueStart = false;
        esperandoOpcion = false;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        ToggleOptions(false);

        if (player != null) player.EnableMovement();
        if (dialogueMark != null) dialogueMark.SetActive(isPlayerInRange);
    }

    private void NextDialogueLine()
    {
        lineIndex++;

        if (dialogueLines != null && lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator ShowLine()
    {
        ToggleOptions(false);
        esperandoOpcion = false;

        dialogueText.text = string.Empty;

        // Seguridad de rango
        if (dialogueLines == null || lineIndex < 0 || lineIndex >= dialogueLines.Length)
        {
            Debug.LogWarning($"[{name}] Índice de diálogo fuera de rango ({lineIndex}).");
            EndDialogue();
            yield break;
        }

        string fullText = dialogueLines[lineIndex].texto ?? string.Empty;

        foreach (char ch in fullText)
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }

        // ¿Tiene opciones esta línea?
        var opciones = dialogueLines[lineIndex].opciones;
        if (opciones != null && opciones.Length > 0)
        {
            esperandoOpcion = true;
            BuildOptions(opciones);
            ToggleOptions(true);
        }
    }

    // ==========================
    // Opciones: UI y Lógica
    // ==========================

    private void BuildOptions(DialogueOption[] opciones)
    {
        if (optionsContainer == null || optionButtonPrefab == null)
        {
            Debug.LogWarning($"[{name}] Falta asignar optionsContainer y/o optionButtonPrefab.");
            return;
        }

        // Limpiar anteriores
        foreach (Transform child in optionsContainer)
            Destroy(child.gameObject);

        // Crear botones
        foreach (var op in opciones)
        {
            var btn = Instantiate(optionButtonPrefab, optionsContainer);
            var label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null) label.text = op.texto;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnElegirOpcion(op));
        }
    }

    private void ToggleOptions(bool show)
    {
        if (optionsContainer != null) optionsContainer.gameObject.SetActive(show);
    }

    private void OnElegirOpcion(DialogueOption opcion)
    {
        // 1) Aplicar efectos a variables globales
        AplicarEfectos(opcion.efecto);

        // 2) Ocultar opciones y permitir continuar
        ToggleOptions(false);
        esperandoOpcion = false;

        // 3) Saltar a la línea indicada o terminar
        if (opcion.siguienteLinea >= 0 && dialogueLines != null && opcion.siguienteLinea < dialogueLines.Length)
        {
            lineIndex = opcion.siguienteLinea;
            StartCoroutine(ShowLine());
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary>
    /// Formato soportado (separado por ';'):
    /// "alineacion=heroico; reputacion+=5; ayudarJuan=true"
    /// Variables de ejemplo mapeadas a GameState:
    ///  - alineacion (string)
    ///  - reputacion (int) -> GameState.I.reputacionConGremio
    ///  - ayudarJuan (bool) -> GameState.I.eligioAyudarAJuan
    /// Extendelo con tus propias variables.
    /// </summary>
    private void AplicarEfectos(string efecto)
    {
        if (string.IsNullOrWhiteSpace(efecto)) return;
        if (GameState.I == null)
        {
            Debug.LogWarning("GameState no encontrado. Agregá GameState a una escena y marcá DontDestroyOnLoad.");
            return;
        }

        var partes = efecto.Split(';');
        foreach (var raw in partes)
        {
            var expr = raw.Trim();
            if (string.IsNullOrEmpty(expr)) continue;

            // +=
            if (expr.Contains("+="))
            {
                var seg = expr.Split(new[] { "+=" }, System.StringSplitOptions.None);
                if (seg.Length != 2) continue;
                var varName = seg[0].Trim();
                int val;
                if (!int.TryParse(seg[1].Trim(), out val)) continue;

                if (varName == "reputacion")
                    GameState.I.reputacionConGremio += val;

                continue;
            }

            // -=
            if (expr.Contains("-="))
            {
                var seg = expr.Split(new[] { "-=" }, System.StringSplitOptions.None);
                if (seg.Length != 2) continue;
                var varName = seg[0].Trim();
                int val;
                if (!int.TryParse(seg[1].Trim(), out val)) continue;

                if (varName == "reputacion")
                    GameState.I.reputacionConGremio -= val;

                continue;
            }

            // =
            if (expr.Contains("="))
            {
                var seg = expr.Split('=');
                if (seg.Length != 2) continue;
                var varName = seg[0].Trim();
                var val = seg[1].Trim();

                switch (varName)
                {
                    case "alineacion":
                        GameState.I.alineacion = val;
                        break;

                    case "ayudarJuan":
                        GameState.I.eligioAyudarAJuan = val.ToLower() == "true";
                        break;

                        // Agregá más mappings acá:
                        // case "otraVariable":
                        //     GameState.I.otraVariable = ...
                        //     break;
                }
            }
        }
    }

    // ==========================
    // Trigger de interacción
    // ==========================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!didDialogueStart && dialogueMark != null) dialogueMark.SetActive(true);
            Debug.Log("Se puede hablar");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (dialogueMark != null) dialogueMark.SetActive(false);
            Debug.Log("No se puede hablar");

            // Si el jugador se va en medio del diálogo, lo cerramos
            if (didDialogueStart) EndDialogue();
        }
    }
}


