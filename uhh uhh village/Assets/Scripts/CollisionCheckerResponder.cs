using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerResponder : MonoBehaviour
{
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
        if (collider.gameObject.tag == "Terrain")
        {
            safeToSpawn = true;
        } else
        {
            safeToSpawn = false;
        }
        Debug.Log("Internally it is " + safeToSpawn);
    }

    
}
