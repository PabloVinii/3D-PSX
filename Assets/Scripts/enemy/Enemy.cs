using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navMesh; // Referência ao NavMeshAgent para navegação
    [SerializeField] private Animator anim; // Referência ao Animator para controlar as animações
    [SerializeField] private GameObject player; // Referência ao jogador
    [SerializeField] private GameObject objHeadDetector; // Objeto detector de colisão com a cabeça
    [SerializeField] private AudioClip[] enemySounds; // Sons do inimigo
    [SerializeField] private AudioSource enemyAudioSource; // AudioSource para reproduzir os sons

    [Header("Settings")]
    [SerializeField] private float playerDistance; // Distância para o jogador
    [SerializeField] private float attackDistance; // Distância para atacar o jogador
    [SerializeField] private float velocity = 5f; // Velocidade do inimigo
    [SerializeField] private int enemyLife = 100; // Vida do inimigo
    [SerializeField] private int enemyDamage = 25; // Dano do inimigo

    private EnemyRagdoll ragdollScript; // Referência ao script EnemyRagdoll para gerenciar o ragdoll
    public bool isDead; // Flag indicando se o inimigo está morto
    //public bool rageMode;

    private void Start()
    {
        isDead = false;
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        ragdollScript = GetComponent<EnemyRagdoll>();
        enemyAudioSource = GetComponent<AudioSource>();

        ragdollScript.DisableRagdoll(); // Desabilita o ragdoll no início
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            playerDistance = Vector3.Distance(transform.position, player.transform.position);
            ChasePlayer(); // Persegue o jogador
            LookToPlayer(); // Olha na direção do jogador

            if (enemyLife <= 0 && !isDead)
            {
                //EnemySoundDeath();
                objHeadDetector.SetActive(false);
                isDead = true;
                StopWalk();
                navMesh.enabled = false;
                ragdollScript.StartRagdoll(); // Inicia o ragdoll ao morrer
            }
        } 
    }

    private void ChasePlayer()
    {
        navMesh.speed = velocity;

        if (playerDistance < attackDistance)
        {
            navMesh.isStopped = true;
            anim.SetTrigger("Attack");
            anim.SetBool("canWalk", false);
            anim.SetBool("stopAttack", false);
            FixEnterRig();
        }
        if (playerDistance >= 3)
        {
            anim.SetBool("stopAttack", true);
        }
        if (anim.GetBool("canWalk"))
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("Attack");
            FixExitRig();
        }
    }

    public void DamagePlayer()
    {
        // player.GetComponent<PlayerController>().health -= enemyDamage;
    }

    public void DamageEnemy(int damage)
    {
        int chance = Random.Range(0, 10);

        if (chance % 2 == 0)
        {
            StopWalk();
        }
        
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
        anim.SetTrigger("takeDamage");
        anim.SetBool("canWalk", false);
        FixEnterRig();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FixEnterRig();
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            FixExitRig();
        }
    }

    private void FixEnterRig()
    {
        ragdollScript.rigid.isKinematic = true;
        ragdollScript.rigid.velocity = Vector3.zero;
    }

    private void FixExitRig()
    {
        ragdollScript.rigid.isKinematic = false;
    }

    public void EnemySoundWalk()
    {
        enemyAudioSource.PlayOneShot(enemySounds[0]);
    }

    public void EnemySoundPain()
    {
        enemyAudioSource.clip = enemySounds[1];
        enemyAudioSource.Play();
    }

    public void EnemySoundScream()
    {
        enemyAudioSource.clip = enemySounds[2];
        enemyAudioSource.Play();
    }

    public void EnemySoundDeath()
    {
        enemyAudioSource.clip = enemySounds[2];
        enemyAudioSource.Play();
    }
}
