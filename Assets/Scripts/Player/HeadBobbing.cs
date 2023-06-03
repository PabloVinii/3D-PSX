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
        initialCameraPosition = transform.localPosition;
    }

    private void Update()
    {
        CalculateHeadBobbing();
    }

    private void CalculateHeadBobbing()
    {
        if (characterController.velocity.magnitude < 0.1f)
        {
            return;
        }

        headBobOffset = Mathf.Repeat(headBobOffset + characterController.velocity.magnitude * headBobFrequency * Time.deltaTime, Mathf.PI * 2f);
        float bobFactor = Mathf.Sin(headBobOffset) * headBobAmplitude;

        Vector3 cameraPosition = initialCameraPosition;
        cameraPosition.y += bobFactor;
        transform.localPosition = cameraPosition;
    }
}
