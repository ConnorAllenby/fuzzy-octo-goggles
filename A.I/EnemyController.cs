using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetColliderState(false);
        SetRigidbodyState(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Die()
    {
        Destroy(gameObject, 5);
        GetComponent<Animator>().enabled = false;
        SetColliderState(true);
        SetRigidbodyState(false);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;

       
    }

    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
    }
}
