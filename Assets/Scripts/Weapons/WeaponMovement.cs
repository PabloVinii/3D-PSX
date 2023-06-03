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

    void Start()
    {
        initialPosition = transform.localPosition;
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // Obtém os valores de rotação do mouse
        float mouseX = -Input.GetAxis("Mouse X") * value;
        float mouseY = -Input.GetAxis("Mouse Y") * value;

        // Limita os valores de rotação para evitar movimentos exagerados
        mouseX = Mathf.Clamp(mouseX, -maxValue, maxValue);
        mouseY = Mathf.Clamp(mouseY, -maxValue, maxValue);

        // Obtém a velocidade do movimento do jogador
        Vector3 playerMovement = playerController.GetMovementSpeed();
        Vector3 playerMovementEffect = new Vector3(playerMovement.x, 0, playerMovement.z) * playerMovementIntensity;

        // Calcula a posição final da arma levando em consideração a rotação do mouse e o movimento do jogador
        Vector3 finalPosition = new Vector3(mouseX, mouseY, 0) + playerMovementEffect;

        // Aplica um suavização à posição da arma para um movimento mais fluido
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothValue);
    }
}
