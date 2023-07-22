using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is to be assigned to flags, stockpiles, warehouses etc.
/// It handles their storage capabilities.
/// </summary>
public class FlagStockpileResponder : MonoBehaviour
{
    public int maxStorageCapacity = 20;
    public bool isFull = false;
    public int storageTaken = 0;
    public List<string> itemList;
    public int VillageID = -5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Small function to deposit apples. Might refactor to allow more universalisation later,
    /// but as of now we only have apples as a resource.
    /// </summary>
    /// <returns>if the operation was successful</returns>
    bool addApple()
    {
        if (storageTaken + 1 <= maxStorageCapacity)
        {
            storageTaken += 1;
            itemList.Add("Apple");
            return true;
        }
        return false;
    }
}
