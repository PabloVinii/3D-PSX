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

    // Update is called once per frame
    public void DisableRagdoll()
    {
        Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < rigs.Length; i++)
        {
            if (rigs[i] == rigid)
            {
                continue;
            }

            ragdollRigids.Add(rigs[i]);
            rigs[i].isKinematic = true;

            Collider col = rigs[i].gameObject.GetComponent<Collider>();
            col.isTrigger = true;
            ragdollColliders.Add(col);
        }
    }

    public void StartRagdoll()
    {
        for (int i = 0; i < ragdollRigids.Count; i++)
        {
            ragdollRigids[i].isKinematic = false;
            ragdollColliders[i].isTrigger = false;
        }

        rigid.isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        StartCoroutine("FinishAnimation");
    }

    IEnumerator FinishAnimation()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().enabled = false;
        this.enabled = false;
    }
}