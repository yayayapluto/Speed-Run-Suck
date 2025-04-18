using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 9f;
    private float fallMultiplier = 1.5f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource gemSFX;

    [Header("Settings")]
    [SerializeField] private float inputThreshold = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        Jump();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Move();
        CheckGrounded();
    }

    void HandleInput()
    {
        moveInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        if (Mathf.Abs(moveInput) > inputThreshold)
        {
            // Gradual horizontal velocity adjustment for smooth transitions
            float targetVelocityX = moveInput * moveSpeed;
            rb.velocity = new Vector2(
                Mathf.Lerp(rb.velocity.x, targetVelocityX, 0.2f), // Smooth horizontal movement
                rb.velocity.y
            );

            // Flip sprite based on movement direction
            GetComponent<SpriteRenderer>().flipX = moveInput < 0;
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            if (jumpSFX != null)
            {
                jumpSFX.Play();
            }
        }

        if (rb.velocity.y < 0) // Falling
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // Jump cut
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void UpdateAnimator()
    {
        animator.SetBool("isRunning", Mathf.Abs(moveInput) > inputThreshold && isGrounded);
        animator.SetBool("isJumping", !isGrounded && rb.velocity.y > 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gem"))
        {
            if (gemSFX != null)
            {
                gemSFX.Play();
            }
        }
    }
}