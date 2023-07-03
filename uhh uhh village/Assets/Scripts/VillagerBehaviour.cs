using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI ;


public class VillagerBehaviour : MonoBehaviour
{
    /*
     ok this is going to be BIG
     
     
     
     
     
     */

    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;
     
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
