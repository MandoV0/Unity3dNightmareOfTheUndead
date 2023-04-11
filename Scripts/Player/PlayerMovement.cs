using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Constant for gravity to the player
    private const float Gravity = -13;

    [Title(label: "References")]
    // The player camera transform thats gets turned up and down when looking
    public Transform playerCameraHolder;
    public Transform feetTransform;

    [Title(label: "Player Stance")]
    // The current player stance
    public PlayerStance playerStance;
    public PlayerStanceSettings standStance = new PlayerStanceSettings(1.67f, 1.8f, new Vector3(0, 0.9f, 0));
    public PlayerStanceSettings crouchStance = new PlayerStanceSettings(1, 1, new Vector3(0, 0.5f, 0));
    // How much smooting is applied when switching stances
    public float playerStanceSmoothing = 0.1f;

    [Title(label: "Movement")]
    public float currentSpeed;
    public float walkSpeed = 4;
    public float sprintSpeed = 6;
    public float crouchSpeed = 2;
    public float airSpeed = 1;
    public float jumpForce = 5;

    [Title(label: "Movement Modifiers")]
    [Range(0f, 1f)]
    [SerializeField]
    private float strafeMultiplier = 0.8f;
    [Range(0f, 1f)]
    [SerializeField]
    private float backwardsWalkMultiplier = 0.5f;
    // How much smoothing is applied to the movement
    [SerializeField]
    [Range(0.0f, 0.3f)]
    private float moveSmoothTime = 0.12f;

    public bool isSprinting;

    private float cameraHeight;
    private float cameraHeightVelocity;
    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;
    private float velocityY;
    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleVelocity;
    private Vector2 moveInput;
    private CharacterController controller;
    // if the player was grounded last frame
    private bool wasGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraHeight = playerCameraHolder.localPosition.y;
    }

    private void Update()
    {
        CalculateSpeed();
        CalculateStance();
        MovePlayer();

        // Was the player grounded this frame
        // (has to be after the MovePlayer function so that we know the last frame)
        wasGrounded = controller.isGrounded;
    }

    /// <summary>
    /// Sets the player stance and transitions between them
    /// </summary>
    private void CalculateStance()
    {
        // Default stance
        var currentStance = standStance;

        if (playerStance == PlayerStance.Crouch)
            currentStance = crouchStance;

        var localPosition = playerCameraHolder.localPosition;
        cameraHeight = Mathf.SmoothDamp(localPosition.y, currentStance.cameraHeight,
            ref cameraHeightVelocity, playerStanceSmoothing);
        localPosition = new Vector3(localPosition.x, cameraHeight,
            localPosition.z);
        playerCameraHolder.localPosition = localPosition;

        controller.height = Mathf.SmoothDamp(controller.height, currentStance.height,
            ref stanceCapsuleVelocity, playerStanceSmoothing);
        controller.center = Vector3.SmoothDamp(controller.center, currentStance.center,
            ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    /// <summary>
    /// Moves the Player in the desired direction
    /// </summary>
    private void MovePlayer()
    {
        Vector2 targetDir = moveInput;

        // Apply Movement Modifiers
        float forwardSpeed = currentSpeed;
        float rightSpeed = currentSpeed;

        if (moveInput.y < 0)
        {
            forwardSpeed *= backwardsWalkMultiplier;
        }

        if (moveInput.x != 0)
        {
            rightSpeed *= strafeMultiplier;
        }

        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        // Apply Force when Grounded so that we dont hop on slopes
        if (controller.isGrounded && !wasGrounded)
        {
            velocityY = -4.5f;
        }

        velocityY += Gravity * Time.deltaTime;

        var playerTransform = transform;
        var velocity = ((playerTransform.forward * currentDir.y * forwardSpeed) + (playerTransform.right * currentDir.x * rightSpeed)) + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Reset out Upwards Velocity when we hit a celling so that we arent stuck to it
        if (hit.moveDirection.y > 0.0f && velocityY > 0.0f)
            velocityY = 0.0f;
    }

    private void CalculateSpeed()
    {
        if (controller.isGrounded)
        {
            if (playerStance == PlayerStance.Stand)
            {
                if (isSprinting)
                    currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, Time.deltaTime * 5);
                else
                    currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * 5);
            }
            else if (playerStance == PlayerStance.Crouch)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, crouchSpeed, Time.deltaTime * 5);
            }
        }
        else
        {
            currentSpeed = airSpeed;
        }
    }

    private bool StanceCheck(float stanceCheckHeight)
    {
        var position = feetTransform.position;
        var radius = controller.radius;

        var start = new Vector3(position.x, position.y + radius + 0.05f,
            position.z);
        var end = new Vector3(position.x,
            position.y - radius - 0.05f + stanceCheckHeight,
            position.z);

        return Physics.CheckCapsule(start, end, radius, 0);
    }

    private void Crouch()
    {
        if (!controller.isGrounded) return;
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(standStance.height)) return;

            if (StanceCheck(crouchStance.height)) return;

            playerStance = PlayerStance.Stand;
            return;
        }

        playerStance = PlayerStance.Crouch;
    }

    private void Jump()
    {
        if (!controller.isGrounded) return;
        // If we want to jump but we are crouching
        // Stand up instead
        if (playerStance == PlayerStance.Crouch)
        {
            Crouch();
        }

        velocityY = jumpForce;
    }

    #region INPUT
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Jump();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Crouch();
        }

    }

    #endregion
}

public enum PlayerStance
{
    Stand,
    Crouch
}

[Serializable]
public class PlayerStanceSettings
{
    public float cameraHeight;
    public float height;
    public Vector3 center;

    public PlayerStanceSettings(float cameraHeight, float height, Vector3 center)
    {
        this.cameraHeight = cameraHeight;
        this.center = center;
        this.height = height;
    }
}