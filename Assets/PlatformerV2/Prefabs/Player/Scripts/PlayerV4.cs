using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerV4 : MonoBehaviour
{
    #region General
    [Header("General Objects")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private ParticleSystem dustParticleSystem;
    #endregion

    #region Running
    [Header("Running")]
    [SerializeField] private float speed = 8;
    private float horizontal;
    private bool isFacingRight = true;
    #endregion

    #region Jumping
    [Header("Jumping")]
    [SerializeField] private float jumpingPower = 18f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter = 0f;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0f;

    private bool jumpCut = false;
    // the bigger the number, the earlier the jump is considered a "full jump" and the jump cut logic doesn't apply
    // 20 is roughly the amount of velocity we start with on a jump
    [SerializeField] private float minVelocityToTriggerAJumpCut = 10.0f;
    [SerializeField] private float minVelocityToIncreasedJumpGravity = 5.0f;
    [SerializeField] private TrailRenderer jumpTrailRenderer;
    [SerializeField] private ParticleSystem jumpPS;
    #endregion

    #region Gravity
    [Header("Gravity")]
    [SerializeField] private float gravityScale = 3;
    [SerializeField] private float jumpCutGravityMultiplier = 1.5f;
    [SerializeField] private float fallGravityMultiplier = 1.5f;
    [SerializeField] private float jumpGravityMultiplier = 0.9f;
    [SerializeField] private float maxFallSpeed = 20.0f;
    [SerializeField] private float aritificialFriction = 0.2f;

    private bool doubleJump = false;
    private bool isJumping = false;
    private bool hasTriedToJump = false;
    #endregion

    #region Dashing
    [Header("Dashing")]
    [SerializeField] private float dashPower = 25.0f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;
    [SerializeField] private TrailRenderer dashTrailRenderer;
    private bool canDash = true;
    private bool isDashing = false;
    #endregion

    #region Wall Sliding
    [Header("Wall Sliding")]
    [SerializeField] private float wallSlidingSpeed = 2f;
    private bool isWallSliding;

    private bool isWallJumping;
    private float wallJumpDirection;
    // Time off the wall to still allow a wall jump
    [SerializeField] private float wallJumpCoyoteTime = 0.2f;
    private float wallJumpCounter;
    [SerializeField] private float wallJumpTimeUntilPlayerRegainsControl = 0.25f;
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 16f);
    #endregion

    private LevelManager levelManager;
    private float gameTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.gravityScale = gravityScale;
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }


        if (!isWallJumping)
        {
            // Flip the player based on direction of movement
            if ((!isFacingRight && horizontal > 0) || (isFacingRight && horizontal < 0))
            {
                flipPlayersDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        // We dont want to update velocity if we're wall jumping or dashing
        if (isDashing || isWallJumping)
        {
            return;
        }

        if (isGrounded())
        {
            if (rigidBody.velocity.y == 0)
            {
                doubleJump = false;
                isJumping = false;
                disableJumpTrail();
            }
            if (jumpCut) jumpCut = false;

            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Tick the jump buffer counter down while not jumping
        jumpBufferCounter -= Time.deltaTime;

        // Tick down wall jump counter
        wallJumpCounter -= Time.deltaTime;

        run();

        addArtificialFriction();
        setGravityDuringJumping();
        wallSlide();
    }

    public void horizontalMove(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void jump(InputAction.CallbackContext context)
    {
        // When the player attempts to jump, reset the buffer counter.
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime;
            hasTriedToJump = true;

            wallJump();
            //if (isWallJumping) return;
        }

        if (hasTriedToJump)
        {
            // first or double jump
            //if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || (doubleJump && !isWallSliding)))
            if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || doubleJump && !isWallSliding))
            {
                //jumpPS.Play();
                print("i jump");
                hasTriedToJump = false;
                isJumping = true;
                enableJumpTrail();
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0.0f);
                rigidBody.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
                doubleJump = !doubleJump;
                jumpBufferCounter = 0;

                // Maybe either reset gravity here, or reset velocity entirely
                // Either of these hopefully ensures the second jump has the same power as the first
            }
        }

        // investigate gravity issues with the jump cut

        //if we release early, add a downward force
        //if (context.canceled && rigidBody.velocity.y > minVelocityToTriggerAJumpCut)
        //{
        //    hasTriedToJump = false;
        //    jumpCut = true;
        //    coyoteTimeCounter = 0f;
        //}
    }

    private void setGravityDuringJumping()
    {
        if (jumpCut)
        {
            print("jump cutting");
            //Higher gravity if jump button released
            setGravity(gravityScale * jumpCutGravityMultiplier);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -maxFallSpeed));
        }
        //else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        else if (isJumping && Mathf.Abs(rigidBody.velocity.y) < minVelocityToIncreasedJumpGravity)
        {
            print("Jumping");
            setGravity(gravityScale * jumpGravityMultiplier);
        }
        else if (rigidBody.velocity.y < 0 && !isWallSliding)
        {
            print("negative velocity");
            //Higher gravity if falling
            setGravity(gravityScale * fallGravityMultiplier);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -maxFallSpeed));
        }
        else
        {
            print("gravity otherwise");
            //Default gravity if standing on a platform or moving upwards
            setGravity(gravityScale);
        }
    }

    private void wallSlide()
    {
        if (isWalled() && !isGrounded() && horizontal != 0)
        {
            isWallSliding = true;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            isWallJumping = false;
            // Direction is opposite to direction we're facing
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpCoyoteTime;
            CancelInvoke(nameof(stopWallJumping));
        }
    }

    private void wallJump()
    {
        // wall jump
        if (wallJumpCounter > 0)
        {

            isWallJumping = true;
            doubleJump = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0.0f);
            rigidBody.AddForce(new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y), ForceMode2D.Impulse);

            wallJumpCounter = 0;
            jumpBufferCounter = 0;
            if (transform.localScale.x != wallJumpDirection)
            {
                flipPlayersDirection();
                enableJumpTrail();
            }

            Invoke(nameof(stopWallJumping), wallJumpTimeUntilPlayerRegainsControl);
        }
    }

    private void stopWallJumping()
    {
        isWallJumping = false;
    }

    private void run()
    {
        float targetSpeed = horizontal * speed;
        float speedDiff = targetSpeed - rigidBody.velocity.x;
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? 1.0f : 1.0f;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, 2) * Mathf.Sign(speedDiff);
        rigidBody.AddForce(movement * Vector2.right);
    }

    private void addArtificialFriction()
    {
        if (isGrounded() && Mathf.Abs(horizontal) < 0.01f)
        {
            float frictionAmount = Mathf.Min(Mathf.Abs(rigidBody.velocity.x), aritificialFriction);
            frictionAmount *= Mathf.Sign(rigidBody.velocity.x);
            rigidBody.AddForce(Vector2.right * -frictionAmount, ForceMode2D.Impulse);
        }
    }

    public void activateDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            hasTriedToJump = false;
            isDashing = true;
            canDash = false;
            setGravity(0);

            if (jumpTrailRenderer.emitting) disableJumpTrail();
            enableDashTrail();
            Vector2 forceForward = isFacingRight ? dashPower * Vector2.right : dashPower * Vector2.left;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(forceForward, ForceMode2D.Impulse);

            Invoke(nameof(resetDashVariables), dashTime);

            Invoke(nameof(enableDash), dashCooldown);
        }
    }

    private void resetDashVariables()
    {
        disableDashTrail();
        setGravity(gravityScale);
        isDashing = false;
        forceEnableDoubleJump();
    }

    private void enableDash()
    {
        canDash = true;
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void flipPlayersDirection()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;

        if (isGrounded())
        {
            createDustEffect();
        }
    }

    public void createDustEffect()
    {
        dustParticleSystem.Play();
    }

    private void setGravity(float scale)
    {
        rigidBody.gravityScale = scale;
    }

    public void forceEnableDoubleJump()
    {
        if (!doubleJump) doubleJump = true;
    }

    public void enableJumpTrail()
    {
        jumpTrailRenderer.emitting = true;
    }

    public void disableJumpTrail()
    {
        jumpTrailRenderer.emitting = false;
    }

    public bool getJumpTrailState()
    {
        return jumpTrailRenderer.emitting;
    }

    public void enableDashTrail()
    {
        dashTrailRenderer.emitting = true;
    }

    public void disableDashTrail()
    {
        dashTrailRenderer.emitting = false;
    }

    public bool getDashTrailState()
    {
        return dashTrailRenderer.emitting;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            levelManager.gameFinished = true;
        } else if (collision.CompareTag("Death"))
        {
            levelManager.restart();
        }
    }

    public void cancelDash()
    {
        isDashing = false;
    }

    public void setDash()
    {
        isDashing = true;
    }

    public bool getDashState()
    {
        return isDashing;
    }
}
