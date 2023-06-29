using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using System.Linq;

public class RockSpawning : MonoBehaviour
{
    
    [SerializeField]
    private GameObject rockPrefab;

    //private int rockList;
    private GameObject spawner;
    private float interval = 3f;
    private int maxAmount = 100;
    [SerializeField]
    private string generatedTag = "";

    // Start is called before the first frame update
    void Start()
    {
        spawner = gameObject;
        generatedTag = "Rock" + FixNumericString("" + gameObject.transform.position.x + gameObject.transform.position.z);
        Debug.Log(generatedTag);

        StartCoroutine(spawnRock(interval, rockPrefab, maxAmount));


        
    }

    // Update is called once per frame
    void Update()
    {
        //newRock = new GameObject


    }

    private IEnumerator spawnRock(float interval, GameObject rock, float limit)
    {
        yield return new WaitForSeconds(interval);

        //rockList = GameObject.FindGameObjectsWithTag(generatedTag).Length();

        if (findMatchingRocks().Length < limit)
        {

            GameObject newRock = Instantiate(rock, new Vector3(
                Random.Range(gameObject.transform.position.x - 5f, gameObject.transform.position.x + 5f),
                gameObject.transform.position.y,
                Random.Range(gameObject.transform.position.z - 5f, gameObject.transform.position.z + 5f)),
                Quaternion.identity);

            //newRock.tag = generatedTag;
            newRock.GetComponent<RockID>().ID = generatedTag;

            Debug.Log("spawning a rock at id " + generatedTag);
        } else {
            Debug.Log("rock limit reached at id " + generatedTag);
        }
        StartCoroutine(spawnRock(interval, rock, limit));
    }


    private string FixNumericString(string badString)
    {
        string goodString = "";
        char[] goodChars = {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};
        

        for (int i = 0; i < badString.Length; i++)
        {
           if (goodChars.Contains(badString[i])) {
                goodString += badString[i];
            }
        }
        return goodString;
        
    }


    private GameObject[] findMatchingRocks()
    {
        GameObject[] matchingRocks = {};

        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Rock"))
        {
            if (i.GetComponent<RockID>().ID == generatedTag)
            {
                matchingRocks.Append<GameObject>(i);
            }
        }

        return matchingRocks;
    }
}
