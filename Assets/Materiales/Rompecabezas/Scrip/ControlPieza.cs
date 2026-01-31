using UnityEngine;

public class ControlPieza : MonoBehaviour
{
    private bool moviendo;
    private bool encajada;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    [Header("Configuracion")]
    public Transform objetivoCorrecto;
    public float margenError = 0.5f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    void OnMouseDown()
    {
        if (!encajada)
        {
            moviendo = true;
            sr.sortingOrder = 10; // Trae la pieza al frente al tocarla
        }
    }

    void OnMouseUp()
    {
        moviendo = false;
        if (objetivoCorrecto == null) return;

        if (Vector3.Distance(transform.position, objetivoCorrecto.position) < margenError)
        {
            transform.position = objetivoCorrecto.position;
            encajada = true;
            sr.sortingOrder = 1; // La manda al fondo al encajar

            // Desactiva su colisionador para que no tape a las demas
            if (col != null) col.enabled = false;

            // Avisa al GameManager
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            if (gm != null) gm.ContarPieza();

            objetivoCorrecto.gameObject.SetActive(false);
        }
        else
        {
            sr.sortingOrder = 5; // Vuelve a su capa normal
        }
    }

    void Update()
    {
        if (moviendo && !encajada)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }
}