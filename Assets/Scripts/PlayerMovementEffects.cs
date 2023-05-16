using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementEffects : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerController playerController;
    private Vector3 initialCameraPosition;

    [Header("Head Bobbing")]
    public Transform cameraHolder;
    public float headBobFrequency = 5f;
    public float headBobAmplitude = 0.05f;
    private float headBobOffset = 0f;

    [Header("Footsteps")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        initialCameraPosition = cameraHolder.localPosition;
    }

    private void Update()
    {
        CalculateHeadBobbing();
        PlayFootstepSounds();
    }

    private void CalculateHeadBobbing()
    {
        float headBobSpeed = characterController.velocity.magnitude * headBobFrequency;
        headBobOffset += headBobSpeed * Time.deltaTime;

        float bobFactor = Mathf.Sin(headBobOffset) * headBobAmplitude;

        Vector3 cameraPosition = initialCameraPosition;
        cameraPosition.y += bobFactor;
        cameraHolder.localPosition = cameraPosition;
    }

    
    private void PlayFootstepSounds()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval)
            {
                if (footstepSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, footstepSounds.Length);
                    footstepAudioSource.PlayOneShot(footstepSounds[randomIndex]);
                }

                footstepTimer = 0f;
            }
        }
    }
}
