using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerController playerController;
    private Vector3 initialCameraPosition;

    [Header("Footsteps")]
    public AudioSource footstepAudioSource; // AudioSource responsável pelos sons de passos
    public AudioClip[] footstepSounds; // Array de sons de passos
    public float footstepInterval = 0.5f; // Intervalo entre os sons de passos
    private float footstepTimer = 0f; // Temporizador para controlar o intervalo
    public float footstepVolume = 1f; // Volume dos sons de passos

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        PlayFootstepSounds();
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
                    footstepAudioSource.PlayOneShot(footstepSounds[randomIndex], footstepVolume);
                }

                footstepTimer = 0f;
            }
        }
    }
}
