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
    }

    private void ChasePlayer()
    {
        navMesh.speed = velocity;

        if (PlayerDistance < AttackDistance)
        {
            navMesh.isStopped = true;
            Debug.Log("atacando");
        }
        else
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
        }
    }
}
