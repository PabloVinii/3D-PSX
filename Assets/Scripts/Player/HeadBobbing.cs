using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [Header("Head Bobbing")]
    public float headBobFrequency = 5f;
    public float headBobAmplitude = 0.05f;
    private float headBobOffset = 0f;
    private CharacterController characterController;
    private Vector3 initialCameraPosition;

    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
    }

    private void Start()
    {
        initialCameraPosition = gameObject.transform.localPosition;
    }

    private void Update()
    {
        CalculateHeadBobbing();
        
    }

    private void CalculateHeadBobbing()
    {
        float headBobSpeed = characterController.velocity.magnitude * headBobFrequency;
        headBobOffset += headBobSpeed * Time.deltaTime;

        float bobFactor = Mathf.Sin(headBobOffset) * headBobAmplitude;

        Vector3 cameraPosition = initialCameraPosition;
        cameraPosition.y += bobFactor;
        gameObject.transform.localPosition = cameraPosition;
    }
}
