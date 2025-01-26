using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerJump))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;

    private Vector2 moveInput;

    private void Awake()
    {
        // Grab references to the other scripts on the same GameObject
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
    }

    // Called by the new Input System when the "Move" action is performed.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        playerMovement.SetMoveInput(moveInput);
    }

    // Called by the new Input System when the "Jump" action is performed.
    public void OnJump(InputValue value)
    {
        // Let the PlayerJump script handle all jump logic
        if (value.isPressed)
        {
            playerJump.HandleJump();
        }
    }
}
