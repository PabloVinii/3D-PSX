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
    [SerializeField] private GameObject bloodParticle;

    [Header("Audios")]
    private AudioSource glockSound;
    [SerializeField] private AudioClip[] glockAudios;

    [Header("Weapon Settings")]
    [SerializeField] private int ammo = 17;
    [SerializeField] private int maxAmmo = 17;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int damage = 20;

    private void Awake() 
    {
        pc = GetComponentInParent<PlayerController>(); 
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        glockSound = GetComponent<AudioSource>();
        defaultInput = pc.defaultInput;
        defaultInput.Character.LeftClick.performed += e => FireWeapon();
        defaultInput.Weapon.Reload.performed += e => ReloadWeapon();
        isFiring = false;   
    }

    private void FireWeapon()
    {
        if (!isFiring && ammo > 0 && !anim.GetBool("onAction"))
        {
            ammo--;
            glockSound.clip = glockAudios[0];
            glockSound.Play();
            isFiring = true;
            StartCoroutine(FireRoutine());
        }
        else if (!isFiring && ammo == 0)
        {
            glockSound.clip = glockAudios[1];
            glockSound.Play();
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
            if (hit.transform.tag == "Enemy")
            {
                if (hit.rigidbody != null && hit.transform.GetComponentInParent<Enemy>().isDead)
                {
                    AddForceToObject(ray, 900);
                }
                else if (hit.transform.GetComponent<Enemy>())
                {
                    hit.transform.GetComponent<Enemy>().DamageEnemy(damage);
                }
                else if (hit.transform.GetComponentInParent<Enemy>())
                {
                    hit.transform.GetComponentInParent<Enemy>().DamageEnemy(damage);
                }
                GameObject instantiateBlood = Instantiate(bloodParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                instantiateBlood.transform.parent = hit.transform;
            }
            else
            {
                InstantiateEffects();

                if (hit.rigidbody != null)
                {
                    AddForceToObject(ray, 500);

                }
            }
        }
        yield return new WaitForSeconds(fireRate);
        isFiring = false;
    }

    private void InstantiateEffects()
    {
        Instantiate(impactEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(smokeEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        GameObject holeObj = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        holeObj.transform.parent = hit.transform;
    }

    private void AddForceToObject(Ray ray, float force)
    {
        Vector3 bulletDir = ray.direction;
        hit.rigidbody.AddForceAtPosition(bulletDir * 500, hit.point);
    }
    private void ReloadWeapon()
    {
        if (!isFiring &&  ammo < maxAmmo && !anim.GetBool("onAction"))
        {
            if (ammo == 0)
            {
                anim.Play("ReloadPistol");
                ammo = maxAmmo;
            }
            else
            {
                anim.Play("ReloadWithAmmoPistol");
                ammo = maxAmmo;
            }
            
        }
    }

    private void UnloadMagSound()
    {
        glockSound.clip = glockAudios[2];
        glockSound.Play();
    }
    private void LoadMagSound()
    {
        glockSound.clip = glockAudios[3];
        glockSound.Play();
    }

    private void SliderSound()
    {
        glockSound.clip = glockAudios[4];
        glockSound.Play();
    }
}
