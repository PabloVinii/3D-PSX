using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent navMesh;
    private Animator anim;
    private GameObject player;

    [Header("Settings")]
    [SerializeField] private float PlayerDistance;
    [SerializeField] private float AttackDistance;
    [SerializeField] private float velocity = 5f;
    [SerializeField] private int enemyLife = 100;
    private EnemyRagdoll ragdollScript;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        ragdollScript = GetComponent<EnemyRagdoll>();

       ragdollScript.DisableRagdoll();
    }

    private void FixedUpdate() {
        PlayerDistance = Vector3.Distance(transform.position, player.transform.position);
        ChasePlayer();
        LookToPlayer();

        if (enemyLife <= 0 && !isDead)
        {
            isDead = true;
            StopWalk();
            ragdollScript.StartRagdoll();
            this.enabled = false;
        }
    }

    private void ChasePlayer()
    {
        navMesh.speed = velocity;

        if (PlayerDistance < AttackDistance)
        {
            navMesh.isStopped = true;
            Debug.Log("atacando");
            anim.SetTrigger("Attack");
            anim.SetBool("canWalk", false);
            anim.SetBool("stopAttack", false);
            FixEnterRig();
        }
        if (PlayerDistance >= 3)
        {
            anim.SetBool("stopAttack", true);
        }
        if(anim.GetBool("canWalk"))
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("Attack");
            FixExitRig();
        }
    }

    public void DamageEnemy(int damage)
    {
        enemyLife -= damage;
    }

    private void LookToPlayer()
    {
        Vector3 directionToLook = player.transform.position - transform.position;
        Quaternion rotate = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, Time.deltaTime * 300);
    }

    private void StopWalk()
    {
        navMesh.isStopped = true;
        anim.SetBool("canWalk", false);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FixEnterRig();
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FixExitRig();
        }
    }

    void FixEnterRig()
    {
       ragdollScript.rigid.GetComponent<Rigidbody>().isKinematic = true;
       ragdollScript.rigid.velocity = Vector3.zero;
    }

    void FixExitRig()
    {
        ragdollScript.rigid.isKinematic = false;
    }

}
