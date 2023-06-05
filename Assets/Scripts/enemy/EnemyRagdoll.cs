using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    private List<Rigidbody> ragdollRigids = new List<Rigidbody>();
    public Rigidbody rigid;
    private List<Collider> ragdollColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();    
    }
    
    // Desativa o ragdoll, tornando os Rigidbodies kinemáticos e os Colliders em triggers
    public void DisableRagdoll()
    {
        // Obtém todos os Rigidbodies na hierarquia do objeto, incluindo o próprio Rigidboy principal
        Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rig in rigs)
        {
            if (rig == rigid)
            {
                continue;
            }

            // Adiciona o Rigidbody à lista de Rigidbodies do ragdoll
            ragdollRigids.Add(rig);
            rig.isKinematic = true;

            // Obtém o Collider do Rigidbody e o configura como um trigger
            Collider col = rig.GetComponent<Collider>();
            col.isTrigger = true;
            ragdollColliders.Add(col);
        }
    }

    // Ativa o ragdoll, tornando os Rigidbodies dinâmicos e os Colliders em não-triggers
    public void StartRagdoll()
    {
        foreach (Rigidbody rig in ragdollRigids)
        {
            rig.isKinematic = false;

            Collider col = rig.GetComponent<Collider>();
            col.isTrigger = false;
            rig.gameObject.layer = 11; // Define a camada do objeto como "Ragdoll" (layer 11)
        }

        rigid.isKinematic = true; // Torna o Rigidboy principal kinemático para desativar seu comportamento físico
        GetComponent<CapsuleCollider>().enabled = false; // Desativa o Collider do objeto principal (assumindo que seja um CapsuleCollider)

        GetComponent<Animator>().enabled = false; // Desativa o componente Animator para interromper as animações
        enabled = false; // Desativa este script para evitar a execução contínua de seu código
    }
}
