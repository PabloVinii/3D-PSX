using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private float smoothValue;
    [SerializeField] private float maxValue;
    private Vector3 initalPosition;

    // Start is called before the first frame update
    void Start()
    {
        initalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float Xaxis = -Input.GetAxis("Mouse X") * value;
        float Yaxis = -Input.GetAxis("Mouse Y") * value;

        Xaxis = Mathf.Clamp(Xaxis, -maxValue, maxValue);
        Yaxis = Mathf.Clamp(Yaxis, -maxValue, maxValue);
        
        Vector3 finalPosition = new Vector3(Xaxis, Yaxis, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initalPosition, Time.deltaTime * smoothValue);
    }
}
