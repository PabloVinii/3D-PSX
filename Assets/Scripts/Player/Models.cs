using System;
using System.Collections.Generic;
using UnityEngine;

public static class Models
{
    #region - Player -

    // Enum para representar as diferentes posturas do jogador
    public enum PlayerStance
    {
        Stand,
        Crouch,
        Prone,
    }

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("Movement - Walking")]
        [Range(0, 50)] public float walkingForwardSpeed; // Velocidade de movimento para frente ao andar
        [Range(0, 50)] public float walkingBackwardSpeed; // Velocidade de movimento para trás ao andar
        [Range(0, 50)] public float walkingStrafeSpeed; // Velocidade de movimento lateral ao andar

        [Header("Movement - Running")]
        [Range(0, 100)] public float runningForwardSpeed; // Velocidade de movimento para frente ao correr
        [Range(0, 100)] public float runningStrafeSpeed; // Velocidade de movimento lateral ao correr

        [Header("Stamina")]
        [Range(0, 100)] public float maxStamina; // Stamina máxima do jogador
        [Range(0, 100)] public float staminaRecovery; // Taxa de recuperação da stamina
        [Range(0, 100)] public float staminaCost; // Custo de stamina ao executar ações

        [Header("Movement Settings")]
        public bool sprintingHold; // Define se o jogador precisa manter o botão de corrida pressionado ou se é um toque para alternar
        public float movementSmoothing; // Suavização do movimento do jogador

        [Header("Jumping")]
        [Range(0, 20)] public float jumpingHeight; // Altura do salto do jogador
        [Range(0, 10)] public float jumpingFalloff; // Decaimento do salto do jogador
        public float fallingSmoothing; // Suavização da queda do jogador

        [Header("Speed Effectors")]
        [Range(0, 5)] public float speedEffector = 1; // Fator de velocidade geral do jogador
        [Range(0, 5)] public float crouchSpeedEffector; // Fator de velocidade ao estar agachado
        [Range(0, 5)] public float proneSpeedEffector; // Fator de velocidade ao estar deitado
        [Range(0, 5)] public float fallingSpeedEffector; // Fator de velocidade durante a queda
    }

    [Serializable]
    public class CharacterStance
    {
        public float cameraHeight; // Altura da câmera em relação ao jogador
        public CapsuleCollider stanceCollider; // Collider usado para a postura
    }
    #endregion
}
