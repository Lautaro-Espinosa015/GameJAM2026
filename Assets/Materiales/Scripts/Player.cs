using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody2D rb2D;
    private Vector2 movementInput;
    public bool canMove = true;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {

        if (canMove)
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
            movementInput = movementInput.normalized;
        }
        else
        {
            movementInput = Vector2.zero; // Forzamos quieto
        }

    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = movementInput*speed;
    }


    public void EnableMovement()
    {
        canMove = true;
        movementInput = Vector2.zero; 
        rb2D.linearVelocity = Vector2.zero;
    }

    public void DisableMovement()
    {
        canMove = false;
        movementInput = Vector2.zero;
        rb2D.linearVelocity = Vector2.zero;
    }


}
