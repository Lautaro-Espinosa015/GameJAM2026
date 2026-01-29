using UnityEngine;

public class Chicos : MonoBehaviour

{

    public float speed = 100f;

    private Rigidbody2D rd2D;
    private Vector2 movementInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rd2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

    }
    private void FixedUpdate()
    {
        rd2D.linearVelocity = movementInput * speed;
    }
}
