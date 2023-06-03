using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cameraHolder;

    [Header("View Settings")]
    [Range(0, 100)] public float viewXSensitivity; // Sensibilidade de rotação no eixo X
    [Range(0, 100)] public float viewYSensitivity; // Sensibilidade de rotação no eixo Y

    public bool ViewXInverted; // Indica se a rotação no eixo X está invertida
    public bool ViewYInverted; // Indica se a rotação no eixo Y está invertida

    [Header("Settings")]
    public float viewClampYMin = -70; // Limite mínimo de rotação no eixo Y
    public float viewClampYMax = 80; // Limite máximo de rotação no eixo Y

    private Vector3 newCharacterRotation;
    private Vector3 newCameraRotation;
    private Inputs defaultInput;
    private Vector2 inputView;
    private PlayerController pc;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    private void Start()
    {
        defaultInput = pc.defaultInput;
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();

        newCharacterRotation = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        CalculateView();
    }

    private void CalculateView()
    {
        // Inversão das entradas
        float invertX = ViewXInverted ? -1f : 1f;
        float invertY = ViewYInverted ? 1f : -1f;

        // Rotação do personagem apenas no eixo Y
        newCharacterRotation.y += viewXSensitivity * invertX * inputView.x * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0f, newCharacterRotation.y, 0f);

        // Rotação da câmera no eixo X com limite
        newCameraRotation.x += viewYSensitivity * invertY * inputView.y * Time.fixedDeltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);
        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }
}
