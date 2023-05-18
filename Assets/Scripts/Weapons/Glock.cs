using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
    private Animator anim;
    private bool isFiring;
    private RaycastHit hit;
    
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject ShootEffect;
    [SerializeField] private GameObject ShootEffectPosition;

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

        GameObject shootEffectObj = Instantiate(ShootEffect, ShootEffectPosition.transform.position, ShootEffectPosition.transform.rotation);
        shootEffectObj.transform.parent = ShootEffectPosition.transform;

        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            InstantiateEffects();

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

    private void InstantiateEffects()
    {
        Instantiate(impactEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(smokeEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        GameObject holeObj = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        holeObj.transform.parent = hit.transform;
    }
}
