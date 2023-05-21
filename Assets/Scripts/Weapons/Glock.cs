using System.Collections;
using UnityEngine;

public class Glock : MonoBehaviour
{
    [Header("Input Settings")]
    private PlayerController pc;
    private Inputs defaultInput;

    private Animator anim;
    private RaycastHit hit;
    private bool isFiring;
    
    [Header("Fire Effects")]
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject ShootEffect;
    [SerializeField] private GameObject ShootEffectPosition;

    private AudioSource fireSound;

    private void Awake() 
    {
        pc = GetComponentInParent<PlayerController>(); 
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        fireSound = GetComponent<AudioSource>();
        defaultInput = pc.defaultInput;
        defaultInput.Character.LeftClick.performed += e => FireWeapon();
        defaultInput.Weapon.Reload.performed += e => ReloadWeapon();
        isFiring = false;   
    }

    private void FireWeapon()
    {
        if (!isFiring && !anim.GetBool("onAction"))
        {
            fireSound.Play();
            isFiring = true;
            StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        anim.Play("FirePistol");

        GameObject shootEffectObj = Instantiate(ShootEffect, ShootEffectPosition.transform.position, ShootEffectPosition.transform.rotation);
        shootEffectObj.transform.parent = ShootEffectPosition.transform;

        if (Physics.Raycast(new Vector3(ray.origin.x + Random.Range(-0.05f, 0.05f), ray.origin.y + Random.Range(-0.05f, 0.05f)
            , ray.origin.z), Camera.main.transform.forward, out hit))
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

    private void ReloadWeapon()
    {
        if (!isFiring && !anim.GetBool("onAction"))
        {
            anim.Play("ReloadPistol");
        }
    }

    private void InstantiateEffects()
    {
        Instantiate(impactEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(smokeEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        GameObject holeObj = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        holeObj.transform.parent = hit.transform;
    }
}
