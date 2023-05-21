using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    [SerializeField] private float value = 1f;
    [SerializeField] private float playerMovementIntensity = 1f;
    [SerializeField] private float smoothValue = 5f;
    [SerializeField] private float maxValue = 2f;

    private Vector3 initialPosition;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = -Input.GetAxis("Mouse X") * value;
        float mouseY = -Input.GetAxis("Mouse Y") * value;

        mouseX = Mathf.Clamp(mouseX, -maxValue, maxValue);
        mouseY = Mathf.Clamp(mouseY, -maxValue, maxValue);

        Vector3 playerMovement = playerController.GetMovementSpeed();
        Vector3 playerMovementEffect = new Vector3(playerMovement.x, 0, playerMovement.z) * playerMovementIntensity;

        Vector3 finalPosition = new Vector3(mouseX, mouseY, 0) + playerMovementEffect;

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothValue);
    }
}
