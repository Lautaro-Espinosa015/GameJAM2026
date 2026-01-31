using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public bool canMove = true;

    private Rigidbody2D rb2D;
    private Vector2 movementInput;

    private AudioSource footstepAudio;

    void Awake()
    {

        rb2D = GetComponent<Rigidbody2D>();
        footstepAudio = GetComponent<AudioSource>();


        if (footstepAudio != null)
        {
            footstepAudio.playOnAwake = false;
            footstepAudio.loop = true;
            footstepAudio.spatialBlend = 0f;
        }
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
            movementInput = Vector2.zero;
        }


        if (footstepAudio != null)
        {
            if (movementInput != Vector2.zero)
            {
                if (!footstepAudio.isPlaying)
                    footstepAudio.Play();
            }
            else
            {

                footstepAudio.Stop();

            }
        }

    }

    void FixedUpdate()
    {
        rb2D.linearVelocity = movementInput * speed;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        movementInput = Vector2.zero;
        rb2D.linearVelocity = Vector2.zero;

        if (footstepAudio != null)
            footstepAudio.Stop();
    }
}
