using System.Collections;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    /*
     This script works HORRIBLY and half the times it does not. I do not
    know why. No one will ever know why. This script is cursed by the ancient
    Maya gods for a reason we will never know.

    TODO:
    Rewrite to spawn trees on terrain in forest areas.

    This script is to be assigned to an empty which would act as a spawn controller.

    */


    [SerializeField] float h = 0f;
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

            if (isSpawnAvailable(new Vector3(coords.x, height + 0.5f, coords.z), collisionCheckerPrefab))
            {

                GameObject newTree = Instantiate(tree, coords,
                    Quaternion.identity);

                newTree.tag = "Tree";


                Debug.Log("spawning a tree");
            }
            else
            {
                Debug.Log("Coords were not safe. Not spawning tree.");
            }

            collisionCheckerPrefab.GetComponent<CollisionCheckerResponder>().safeToSpawn = true;
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
            Debug.Log("Unsafe to spawn tree! " +
                      collisionChecker.GetComponent<CollisionCheckerResponder>().safeToSpawn);

            toreturn = false;
        }

        return toreturn;
    }

    private IEnumerator awaitCollisionCheck(GameObject checker)
    {
        yield return new WaitForSeconds(0.1f);


    }


}
