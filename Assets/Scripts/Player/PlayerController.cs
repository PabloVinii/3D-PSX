using System.Collections;
using System.Collections.Generic;
using static Models;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Inputs defaultInput;
    private Vector2 inputMovement;

    [Header("References")]
    public Transform cameraHolder;
    public Transform feetTransform;

    [Header("Settings")]
    public LayerMask playerMask;
    public PlayerSettingsModel playerSettings;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;

    public CharacterStance playerStandStance;
    public CharacterStance playerCrouchStance;
    public CharacterStance playerProneStance;
    private float stanceCheckErrorMargin = 0.05f;

    public float cameraHeight;
    private float cameraHeightVelocity;

    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;

    private bool isSprinting;
    public float currentStamina;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    private void Awake()
    {
        defaultInput = new Inputs();
        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();
        defaultInput.Character.Crouch.performed += e => Crouch();
        defaultInput.Character.Prone.performed += e => Prone();
        defaultInput.Character.Sprint.performed += e => ToggleSprinting();
        defaultInput.Character.SprintReleased.performed += e => StopSprinting();

        defaultInput.Enable();

        characterController = GetComponent<CharacterController>();

        cameraHeight = cameraHolder.localPosition.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentStamina = playerSettings.maxStamina;
    }

    private void Update()
    {
        CalculateMovement();
        CalculateJump();
        CalculateStance();
        UpdateStamina();
    }

    private void CalculateMovement()
    {
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
        }

        float verticalSpeed = playerSettings.walkingForwardSpeed;
        float horizontalSpeed = playerSettings.walkingStrafeSpeed;

        if (isSprinting && currentStamina >= 0.1f)
        {
            verticalSpeed = playerSettings.runningForwardSpeed;
            horizontalSpeed = playerSettings.runningStrafeSpeed;
            currentStamina -= playerSettings.staminaCost * Time.deltaTime;
        }

        if (!characterController.isGrounded)
        {
            playerSettings.speedEffector = playerSettings.fallingSpeedEffector;
        }
        else if (playerStance == PlayerStance.Crouch)
        {
            playerSettings.speedEffector = playerSettings.crouchSpeedEffector;
        }
        else if (playerStance == PlayerStance.Prone)
        {
            playerSettings.speedEffector = playerSettings.proneSpeedEffector;
        }
        else
        {
            playerSettings.speedEffector = 1;
        }

        verticalSpeed *= playerSettings.speedEffector;
        horizontalSpeed *= playerSettings.speedEffector;

        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horizontalSpeed * inputMovement.x * Time.deltaTime, 0, verticalSpeed * inputMovement.y * Time.deltaTime),
            ref newMovementSpeedVelocity, characterController.isGrounded ? playerSettings.movementSmoothing : playerSettings.fallingSmoothing);
        Vector3 movementSpeed = transform.TransformDirection(newMovementSpeed);

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }

        movementSpeed.y += playerGravity;
        movementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(movementSpeed);
    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFalloff);
    }

    private void CalculateStance()
    {
        Dictionary<PlayerStance, CharacterStance> stanceDictionary = new Dictionary<PlayerStance, CharacterStance>()
        {
            { PlayerStance.Stand, playerStandStance },
            { PlayerStance.Crouch, playerCrouchStance },
            { PlayerStance.Prone, playerProneStance }
        };

        CharacterStance targetStance = stanceDictionary[playerStance];

        // Suavização da altura da câmera
        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, targetStance.cameraHeight, ref cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);

        // Suavização da altura e centro do CharacterController
        SmoothDampCharacterControllerHeight(targetStance.stanceCollider.height, targetStance.stanceCollider.center);
    }

    private void SmoothDampCharacterControllerHeight(float targetHeight, Vector3 targetCenter)
    {
        characterController.height = Mathf.SmoothDamp(characterController.height, targetHeight, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, targetCenter, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    private void Jump()
    {
        if (!characterController.isGrounded || playerStance == PlayerStance.Prone)
        {
            return;
        }

        if (playerStance == PlayerStance.Crouch)
        {
            float standHeight = playerStandStance.stanceCollider.height;

            if (CanChangeStanceToStand(standHeight))
            {
                return;
            }

            playerStance = PlayerStance.Stand;
            return;
        }

        ApplyJumpingForce();
    }

    private bool CanChangeStanceToStand(float standHeight)
    {
        if (StanceCheck(standHeight))
        {
            return true;
        }

        return false;
    }

    private void ApplyJumpingForce()
    {
        jumpingForce = Vector3.up * playerSettings.jumpingHeight;
        playerGravity = 0;
    }

    private void Crouch()
    {
        if (playerStance == PlayerStance.Crouch)
        {
            float standHeight = playerStandStance.stanceCollider.height;

            if (CanChangeStanceToStand(standHeight))
            {
                return;
            }

            playerStance = PlayerStance.Stand;
            return;
        }
        else
        {
            float crouchHeight = playerCrouchStance.stanceCollider.height;

            if (CanChangeStanceToStand(crouchHeight))
            {
                return;
            }

            playerStance = PlayerStance.Crouch;
        }
    }

    private void Prone()
    {
        playerStance = PlayerStance.Prone;
    }

    private bool StanceCheck(float stanceCheckHeight)
    {
        // Define os pontos de início e fim da cápsula de verificação de colisão
        Vector3 capsuleStart = new Vector3(feetTransform.position.x, feetTransform.position.y + characterController.radius + stanceCheckErrorMargin, feetTransform.position.z);
        Vector3 capsuleEnd = new Vector3(feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);

        // Realiza a verificação de colisão usando uma cápsula
        return Physics.CheckCapsule(capsuleStart, capsuleEnd, characterController.radius, playerMask);
    }

    private void ToggleSprinting()
    {
        if (playerStance == PlayerStance.Stand)
        {
            if (inputMovement.y <= 0.2f)
            {
                isSprinting = false;
                return;
            }

            isSprinting = !isSprinting;
        }  
    }

    private void StopSprinting()
    {
        // Verifica se a configuração de sprint contínuo está desativada
        if (!playerSettings.sprintingHold)
        {
            isSprinting = false;
        }
    }

    private void UpdateStamina()
    {
        if (!isSprinting && currentStamina < playerSettings.maxStamina)
        {
            currentStamina += playerSettings.staminaRecovery * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, playerSettings.maxStamina);
        }
    }
}
