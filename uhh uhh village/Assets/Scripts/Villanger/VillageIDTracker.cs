using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageIDTracker : MonoBehaviour
{
    public int latestID = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Creates a new village id.
    /// </summary>
    /// <returns>new village id</returns>
    public int CreateNewVillage()
    {
        latestID++;
        return latestID;
    }
}
