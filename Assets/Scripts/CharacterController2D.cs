using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
	// 1. Movement Parameters
	[Header("Movement Parameters")]
	[SerializeField] private float walkSpeed = 5f;
	[SerializeField] private float runSpeed = 8f;
	[SerializeField] private float acceleration = 10f;
	[SerializeField] private float deceleration = 15f;

	// 2. Jump Parameters
	[Header("Jump Parameters")]
	[SerializeField] private float jumpVelocity = 12f;
	[SerializeField] private float gravityScale = 3f;
	[SerializeField] private float maxJumpHeight = 4f;

	// 3. Double Jump Parameters
	[Header("Double Jump Parameters")]
	[SerializeField] private int maxDoubleJumps = 1;
	private int doubleJumpsLeft;

	// 4. Dash Parameters
	[Header("Dash Parameters")]
	[SerializeField] private float dashDistance = 3f;
	[SerializeField] private float dashCooldown = 1.5f;
	[SerializeField] private float dashStaminaCost = 20f;
	private float dashTimer = 0f;

	// 5. Wall Jump Parameters
	[Header("Wall Jump Parameters")]
	[SerializeField] private float wallJumpVelocity = 10f;
	[SerializeField] private float wallJumpDirection = 5f; // Horizontal velocity when wall jumping

	// 6. Wall Slide Parameters
	[Header("Wall Slide Parameters")]
	[SerializeField] private float wallSlideSpeed = 2f;

	// 7. Glide Parameters
	// [Header("Glide Parameters")]
	// [SerializeField] private float glideDuration = 2f;
	// [SerializeField] private float glideStaminaCost = 15f;
	// private float glideTimer = 0f;
	// private bool isGliding = false;

	// 8. Stamina Parameters
	// [Header("Stamina Parameters")]
	// [SerializeField] private float maxStamina = 100f;
	// [SerializeField] private float stami naRegenRate = 10f;
	// private float currentStamina;

	// 9. Ground and Wall Check
	[Header("Ground and Wall Check")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.2f;
	[SerializeField] private LayerMask groundLayer;

	[SerializeField] private Transform wallCheck;
	[SerializeField] private float wallCheckDistance = 0.5f;
	private bool isGrounded;
	private bool isTouchingWall;
	private bool isTouchingWallLeft;
	private bool isTouchingWallRight;

	// 10. Movement Variables
	private float horizontalInput;
	private float targetSpeed;
	private float velocityX;

	private Rigidbody2D rb;

	// 11. State Variables
	private bool canDoubleJump;
	private bool isFacingRight = true;

	// 12. Input Variables
	private bool jumpPressed;
	private bool dashPressed;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = gravityScale;
	}

	private void Start()
	{
		// currentStamina = maxStamina;
		doubleJumpsLeft = maxDoubleJumps;
	}

	private void Update()
	{
		HandleInput();
		HandleDashCooldown();
		// HandleStaminaRegen();
	}

	private void FixedUpdate()
	{
		CheckGrounded();
		CheckWall();
		HandleMovement();
		HandleJump();
		// HandleDash();
		HandleWallSlide();
		// HandleGlide();
		HandleWallJumping();
	}

	/// <summary>
	/// Handles player input.
	/// </summary>
	private void HandleInput()
	{
		// Horizontal Movement Input
		horizontalInput = Input.GetAxisRaw("Horizontal");
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			jumpPressed = true;
		}

		// Reset double jumps when grounded
		if (isGrounded)
		{
			doubleJumpsLeft = maxDoubleJumps;
		}

		// Glide Activation
		// if (Input.GetKey(KeyCode.Space) && rb.velocity.y < 0)
		// {
		// 	if (currentStamina >= glideStaminaCost && glideTimer <= 0f && !isGliding)
		// 	{
		// 		isGliding = true;
		// 		glideTimer = glideDuration;
		// 		currentStamina -= glideStaminaCost;
		// 	}
		// }
	}

	/// <summary>
	/// Handles movement including walking and running.
	/// </summary>
	private void HandleMovement()
	{
		// Determine target speed based on input and run key
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			targetSpeed = runSpeed * horizontalInput;
		}
		else
		{
			targetSpeed = walkSpeed * horizontalInput;
		}

		// Calculate acceleration or deceleration
		if (Mathf.Abs(targetSpeed) > Mathf.Abs(velocityX))
		{
			velocityX += acceleration * Time.fixedDeltaTime * Mathf.Sign(targetSpeed);
		}
		else
		{
			velocityX = Mathf.MoveTowards(velocityX, targetSpeed, deceleration * Time.fixedDeltaTime);
		}

		// Apply horizontal velocity
		rb.velocity = new Vector2(velocityX, rb.velocity.y);

		// Flip character sprite based on movement direction
		if (horizontalInput > 0 && !isFacingRight)
		{
			Flip();
		}
		else if (horizontalInput < 0 && isFacingRight)
		{
			Flip();
		}
	}

	/// <summary>
	/// Handles jumping and double jumping.
	/// </summary>
	private void HandleJump()
	{
		if (jumpPressed)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (isGrounded)
				{
					rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
					Debug.Log(isGrounded);
				}
				else if (doubleJumpsLeft > 0)
				{
					rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
					doubleJumpsLeft--;
				}
			}
		}

		// Variable Jump Height
		if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
		{
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
		}
	}

	/// <summary>
	/// Handles dashing mechanics.
	/// </summary>
	// private void HandleDash()
	// {
	// 	if (dashTimer > 0f)
	// 	{
	// 		dashTimer -= Time.fixedDeltaTime;
	// 	}

	// 	if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
	// 	{
	// 		if (currentStamina >= dashStaminaCost && dashTimer <= 0f)
	// 		{
	// 			Dash(horizontalInput);
	// 			currentStamina -= dashStaminaCost;
	// 			dashTimer = dashCooldown;
	// 		}
	// 	}
	// }

	/// <summary>
	/// Executes the dash movement.
	/// </summary>
	/// <param name="direction">Direction to dash.</param>
	// private void Dash(float direction)
	// {
	// 	Vector2 dashVelocity = new Vector2(dashDistance * direction, rb.velocity.y);
	// 	rb.velocity = dashVelocity;
	// }

	/// <summary>
	/// Handles wall sliding mechanics.
	/// </summary>
	private void HandleWallSlide()
	{
		if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
		{
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
		}
	}

	/// <summary>
	/// Handles gliding mechanics.
	/// </summary>
	// private void HandleGlide()
	// {
	// 	if (isGliding)
	// 	{
	// 		if (glideTimer > 0f)
	// 		{
	// 			rb.gravityScale = gravityScale * 0.5f; // Reduce gravity during glide
	// 			glideTimer -= Time.fixedDeltaTime;
	// 		}
	// 		else
	// 		{
	// 			isGliding = false;
	// 			rb.gravityScale = gravityScale; // Reset gravity
	// 		}
	// 	}
	// }

	/// <summary>
	/// Checks if the player is grounded.
	/// </summary>
	private void CheckGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
	}

	/// <summary>
	/// Checks if the player is touching a wall.
	/// </summary>
	private void CheckWall()
	{
		RaycastHit2D hitLeft = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, groundLayer);
		RaycastHit2D hitRight = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, groundLayer);

		isTouchingWallLeft = hitLeft.collider != null;
		isTouchingWallRight = hitRight.collider != null;

		isTouchingWall = isTouchingWallLeft || isTouchingWallRight;
	}

	/// <summary>
	/// Flips the character's facing direction.
	/// </summary>
	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 scaler = transform.localScale;
		scaler.x *= -1;
		transform.localScale = scaler;
	}

	/// <summary>
	/// Handles stamina regeneration.
	/// </summary>
	// private void HandleStaminaRegen()
	// {
	// 	if ((!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) && !isGliding)
	// 	{
	// 		currentStamina += staminaRegenRate * Time.fixedDeltaTime;
	// 		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
	// 	}
	// }

	/// <summary>
	/// Handles dash cooldown management.
	/// </summary>
	private void HandleDashCooldown()
	{
		// Dash cooldown is already managed in HandleDash()
		// This method can be used for additional cooldown-related logic if needed
	}

	/// <summary>
	/// Handles wall jumping mechanics.
	/// </summary>
	private void HandleWallJumping()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isTouchingWall && !isGrounded)
		{
			float wallDirection = isTouchingWallLeft ? 1f : -1f;
			rb.velocity = new Vector2(wallJumpDirection * wallDirection, wallJumpVelocity);
			doubleJumpsLeft = maxDoubleJumps; // Reset double jumps upon wall jump
		}
	}
}