using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaManager : MonoBehaviour
{
    public Slider staminaSlider;
    public float fadeDuration = 0.5f; // Duração do efeito de fade

    private PlayerController playerController;
    private CanvasGroup staminaCanvasGroup;
    private Coroutine fadeCoroutine;
    private bool fadingIn; // Variável para controlar se está ocorrendo um fade in

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        staminaCanvasGroup = staminaSlider.GetComponent<CanvasGroup>();
        staminaCanvasGroup.alpha = 0f; // Começa com alpha 0 para esconder a barra de estamina
        fadingIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldFadeIn = playerController.currentStamina < 100; // Verifica se a stamina está abaixo de 100

        // Verifica se a condição para fade in ou fade out é atendida
        if (shouldFadeIn && !fadingIn)
        {
            // Cancela o fade anterior (se houver)
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            // Inicia o fade in
            fadingIn = true;
            fadeCoroutine = StartCoroutine(UpdateStaminaFade(0f, 1f));
        }
        else if (!shouldFadeIn && fadingIn)
        {
            // Cancela o fade anterior (se houver)
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            // Inicia o fade out
            fadingIn = false;
            fadeCoroutine = StartCoroutine(UpdateStaminaFade(1f, 0f));
        }

        // Atualiza o valor do slider para acompanhar a currentStamina
        staminaSlider.value = playerController.currentStamina;
    }

    IEnumerator UpdateStaminaFade(float startAlpha, float targetAlpha)
    {
        // Fade in ou fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            staminaCanvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Define o alpha final
        staminaCanvasGroup.alpha = targetAlpha;
        fadeCoroutine = null;
    }
}
