using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
    private Animator anim;
    private bool isFiring;
    private RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        isFiring = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        anim.Play("FirePistol");

        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            if (hit.transform.tag == "DragObject")
            {
                Vector3 bulletDir = ray.direction;
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(bulletDir * 500, hit.point);
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        isFiring = false;
    }

}
