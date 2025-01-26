using System.Collections;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float wallJumpCooldown = 0.2f;
    [SerializeField] private float kickOffForce = 1f;

    [Header("Apex Settings")]
    [SerializeField] float apexBoost = 5f;
    [SerializeField] float zeroGFloat = 0.15f;
    [SerializeField] float gravityScaleAtStart = 2f;

    [Header("References")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerFeetCollider;
    [SerializeField] private BoxCollider2D playerLeftArmCollider;
    [SerializeField] private BoxCollider2D playerRightArmCollider;

    private float wallJumpTimer;
    private bool hasApexBoosted = false;  // Track if apex boost was used this jump

    private void Update()
    {
        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }

        HandleApexBoost();
    }

    public void HandleJump()
    {
        // Check if we can jump
        if (!CanJump()) { return; }

        // If left arm is touching ground/wall -> wall jump to the right
        if (playerLeftArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            WallJump(-1);
        }
        // If right arm is touching ground/wall -> wall jump to the left
        else if (playerRightArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            WallJump(1);
        }
        else
        {
            // Normal jump from ground
            Vector2 velocity = playerRigidbody.linearVelocity;
            velocity.y = jumpSpeed; // Or add jumpSpeed to existing velocity
            playerRigidbody.linearVelocity = velocity;
            hasApexBoosted = false; // reset apex boost for new jump
        }
    }

    private bool CanJump()
    {
        // Basic check: if feet or arms are not on ground, we can’t jump
        bool touchingFeet = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool touchingLeftArm = playerLeftArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool touchingRightArm = playerRightArmCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // If not touching ANY ground with feet or arms, can't jump
        if (!touchingFeet && !touchingLeftArm && !touchingRightArm)
        {
            return false;
        }

        return true;
    }

    private void WallJump(int direction)
    {
        // Example: push away from the wall
        playerRigidbody.gravityScale = gravityScaleAtStart;
        playerRigidbody.linearVelocity = new Vector2(kickOffForce * direction, jumpSpeed);

        wallJumpTimer = wallJumpCooldown;
        hasApexBoosted = false;
    }

    private void HandleApexBoost()
    {
        // We only want an apex boost if we're currently going downward 
        // after a genuine jump (not just stepping off a ledge),
        // and we haven't used apex boost yet.

        // Check if we're in the air (not grounded with feet).
        bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded) { return; } // If on ground, no apex logic needed (also resets if you want)

        // If we’re moving downward, haven't apex-boosted yet
        if (!hasApexBoosted && playerRigidbody.linearVelocityY < 0f)
        {
            DoApexBoost();
        }
    }

    private void DoApexBoost()
    {
        hasApexBoosted = true;

        // Add a horizontal push in the direction of current velocity.x
        float xVel = playerRigidbody.linearVelocityX;
        if (Mathf.Abs(xVel) > 0.01f)
        {
            float sign = Mathf.Sign(xVel);
            playerRigidbody.AddForce(Vector2.right * apexBoost * sign, ForceMode2D.Impulse);
        }

        StartCoroutine(HandleZeroGravity());
    }

    private IEnumerator HandleZeroGravity()
    {
        float originalGravity = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = 0f;

        float elapsedTime = zeroGFloat;
        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        // Restore gravity
        playerRigidbody.gravityScale = originalGravity;
    }
}
