using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private PlayerInput input;
    private Animator anim;
    private BoxCollider2D boxCol;

    [Header("Jump")]
    public float jumpForce = 10f;
    public int maxJumpCount = 2;
    public int jumpCount;

    [Header("Collider")]
    private Vector2 normalSize;
    private Vector2 normalOffset;

    [SerializeField] private Vector2 slideSize;
    [SerializeField] private Vector2 slideOffset;


    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkDistance = 1f;
    public LayerMask groundLayer;

    [Header("Status")]
    public bool isJump;
    public bool isDoubleJump;
    public bool isSliding;
    public bool isGrounded;
    public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();

        jumpCount = maxJumpCount;

        normalSize = boxCol.size;
        normalOffset = boxCol.offset;
    }

    void Update()
    {
        CheckGround();
        CheckLanding();
        HandleInput();

        anim.SetBool("isJump", isJump);
        anim.SetBool("isDoubleJump", isDoubleJump);
        anim.SetBool("isSliding", isSliding);
    }

    public void Jump()
    {
        if (jumpCount == 0) return;
        if (isDead) return;

        if (jumpCount == maxJumpCount)
            isJump = true;

        else
        {
            isJump = false;
            isDoubleJump = true;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpCount--;
    }

    public void SlideStart()
    {
        isSliding = true;
        boxCol.size = slideSize;
        boxCol.offset = slideOffset;
    }

    public void EndSlide()
    {
        isSliding = false;
        boxCol.size = normalSize;
        boxCol.offset = normalOffset;
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
        {
            isJump = false;
            isDoubleJump = false;
            jumpCount = maxJumpCount;
        }
    }

    void HandleInput()
    {
        if (isDead) return;

        if (input.jumpPressed)
            Jump();

        if (input.slidePressed && !isJump && !isDoubleJump)
            SlideStart();

        if (isSliding && (!input.slidePressed || isJump))
            EndSlide();
    }
}
