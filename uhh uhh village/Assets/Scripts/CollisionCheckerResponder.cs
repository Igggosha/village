using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerResponder : MonoBehaviour
{
    [SerializeField] GameObject debugCollider;
    public bool safeToSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(UnityEngine.Collider collider)
    {
        debugCollider = collider.gameObject;
        if (collider.gameObject.tag == "Tree" || collider.gameObject.tag == "Rock")
        {
            safeToSpawn = false;
        } else
        {
            safeToSpawn = true;
        }
        Debug.Log("Internally it is " + safeToSpawn);
    }

    
}
