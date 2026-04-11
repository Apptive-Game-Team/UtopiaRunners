using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private PlayerInput input;

    [Header("Jump")]
    public float jumpForce = 10f;
    public int maxJumpCount = 2;
    public int jumpCount;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkDistance = 1f;
    public LayerMask groundLayer;

    [Header("Status")]
    public bool isGrounded;
    public bool isSliding;
    public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        jumpCount = maxJumpCount;
    }

    void Update()
    {
        CheckGround();
        CheckLanding();
        HandleInput();
    }

    public void Jump()
    {
        if (jumpCount == 0) return;
        //if (isDead) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpCount--;
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            checkDistance,
            groundLayer
        );
    }

    private void CheckLanding()
    {
        if (rb.linearVelocityY > 0.01f) return;

        if (isGrounded)
            jumpCount = maxJumpCount;
    }

    void HandleInput()
    {
        //if (isDead) return;

        if (input.jumpPressed)
            Jump();
    }
}
