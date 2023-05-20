using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaManager : MonoBehaviour
{
    public Slider staminaSlider;
    private PlayerController playerController;
 
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        staminaSlider.value = playerController.currentStamina;
    }
}
