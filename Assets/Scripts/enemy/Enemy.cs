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

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDistance = Vector3.Distance(transform.position, player.transform.position);

        ChasePlayer();
        LookToPlayer();
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
        }
    }

    private void LookToPlayer()
    {
        Vector3 directionToLook = player.transform.position - transform.position;
        Quaternion rotate = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, Time.deltaTime * 300);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

}
