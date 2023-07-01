using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] float h = 0.04999994f;
    [SerializeField] int treeLimit = 100;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject collisionCheckerPrefab;
    private Vector3 coords;
    [SerializeField] private float interval = 3f;

    GameObject[] treeList;

    private bool toreturn = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnTree(interval, treePrefab, treeLimit, h));
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    private IEnumerator spawnTree(float interval, GameObject tree, float limit, float height)
    {
        yield return new WaitForSeconds(interval);

        treeList = GameObject.FindGameObjectsWithTag("Tree");
        if (treeList.Length < limit)
        {
            coords = new Vector3(
                Random.Range(-50f, 50f),
                height,
                Random.Range(-50f, 50f));

            if (isSpawnAvailable(coords, collisionCheckerPrefab))
            {

                GameObject newTree = Instantiate(tree, coords,
                    Quaternion.identity);

                newTree.tag = "Tree";


                Debug.Log("spawning a tree");
            } else
            {
                Debug.Log("Coords were not safe. Not spawning tree.");
            }
        }
        else
        {
            Debug.Log("tree limit reached, not spawning");
        }

        StartCoroutine(spawnTree(interval, tree, limit, height));
    }


    private bool isSpawnAvailable(Vector3 coords, GameObject collisionChecker)
    {
        collisionChecker.transform.position = coords;

        if (collisionChecker.GetComponent<CollisionCheckerResponder>().safeToSpawn)
        {
            Debug.Log("Safe to spawn tree! " + collisionChecker.GetComponent<CollisionCheckerResponder>().safeToSpawn);
            
            toreturn = true;

        }
        else
        {
            Debug.Log("Unsafe to spawn tree! " + collisionChecker.GetComponent<CollisionCheckerResponder>().safeToSpawn);
            
            toreturn = false;
        }
        return toreturn;
    }

    private IEnumerator awaitCollisionCheck (GameObject checker)
    {
        yield return new WaitForSeconds(0.1f);

        
    }

    
}
