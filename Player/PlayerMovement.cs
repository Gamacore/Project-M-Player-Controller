using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeedX = 15f;
    [SerializeField] float groundAccelX = 0.4f;
    [SerializeField] float groundDecelX = 0.3f; // Not shown in example but keep if needed
    [SerializeField] float airAccelX = 0.1f;
    [SerializeField] float airDecelX = 0.1f;    // Not shown in example but keep if needed
    [SerializeField] AnimationCurve accelCurve = default;
    [SerializeField] float maxCurveTime = 1f;
    [SerializeField] float velExp = 1f;
    [SerializeField] float gravityScaleAtStart = 2f;

    [Header("References")]
    [SerializeField] private BoxCollider2D playerFeetCollider;
    [SerializeField] private BoxCollider2D playerLeftArmCollider;
    [SerializeField] private BoxCollider2D playerRightArmCollider;
    [SerializeField] private Rigidbody2D playerRigidbody;

    private Vector2 moveInput;
    private float timeMovingHorizontally;
    private float lastMoveDir = 0f;

    private void Start()
    {
        // (Optional) If you prefer to do it in code rather than the inspector:
        // playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = gravityScaleAtStart;
    }

    private void Update()
    {
        HandleMovementUpdate();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void HandleMovementUpdate()
    {
        bool isTouchingWall = playerLeftArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) 
                           || playerRightArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isTouchingWall)
        {
            MoveMath(isGrounded);
        }
        else
        {
            WallMove(isGrounded);
        }
    }

    private void MoveMath(bool grounded)
    {
        float currentSpeedX = playerRigidbody.linearVelocityX;
        float currentMoveDir = Mathf.Sign(moveInput.x);

        // Reset time if you switch directions
        if (Mathf.Abs(currentMoveDir) > 0.01f && currentMoveDir != lastMoveDir)
        {
            timeMovingHorizontally = 0f;
        }

        // Increment time and clamp to max
        timeMovingHorizontally += Time.deltaTime;
        timeMovingHorizontally = Mathf.Min(timeMovingHorizontally, maxCurveTime);

        float timeNormalized = timeMovingHorizontally / maxCurveTime;
        float curveVal = accelCurve.Evaluate(timeNormalized);

        float targetSpeedX = moveInput.x * moveSpeedX;
        float diffX = targetSpeedX - currentSpeedX;

        // Use your exponent logic if you wish
        float accelX = grounded ? groundAccelX : airAccelX;
        float forceX = Mathf.Clamp(
            Mathf.Pow(Mathf.Abs(diffX) * curveVal * accelX, velExp),
            -30f, 30f
        ) * Mathf.Sign(diffX);

        // Apply force to move horizontally
        playerRigidbody.AddForce(Vector2.right * forceX);

        lastMoveDir = Mathf.Abs(moveInput.x) > 0.01f ? currentMoveDir : 0f;
    }

    private void WallMove(bool grounded)
    {
        // If you have a cooldown or a "wall cling" mechanic
        if (playerLeftArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) 
            && moveInput.x == 1 && !grounded)
        {
            // Cling to wall: zero gravity, no horizontal movement
            playerRigidbody.gravityScale = 0f;
            playerRigidbody.linearVelocity = Vector2.zero;
        }
        else if (playerRightArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))
            && moveInput.x == -1 && !grounded)
        {
            playerRigidbody.gravityScale = 0f;
            playerRigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            // Restore normal gravity if not clinging
            playerRigidbody.gravityScale = gravityScaleAtStart;
            MoveMath(grounded);
        }
    }
}
