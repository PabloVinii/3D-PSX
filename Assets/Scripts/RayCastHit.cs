using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastHit : MonoBehaviour
{
    public float targetDistance;
    public GameObject dragObj; 
    public GameObject takeObj;
    private RaycastHit hit;

    void Start()
    {
        // Ignora a colisão entre a camada do jogador (câmera) e a camada do Raycast
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5))
            {
                targetDistance = hit.distance;
                if (hit.transform.gameObject.tag == "DragObject")
                {
                    dragObj = hit.transform.gameObject;
                }
                else if (hit.transform.gameObject.tag == "TakeObject")
                {
                    takeObj = hit.transform.gameObject;
                }
                else
                {
                    dragObj = null;
                    takeObj = null;
                }
            }

        }
    }
}
