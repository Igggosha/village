using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLifeCycle : MonoBehaviour
{
    //[SerializeField] private int MaxLifeTime;
    [SerializeField] private float MaxSize;
    [SerializeField] private bool isAppleTree = false;
    [SerializeField] private bool isFlourishing = false; // worst name i could think off for this but this is to indicate if an apple tree has apples already

    [SerializeField] private Material transparent;
    [SerializeField] private Material applered;
    [SerializeField] private float timeModifier = 10f;
    // Start is called before the first frame update
    void Start()
    {
        MaxSize =  Random.Range(0.5f, 5f);
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (gameObject.transform.GetChild(i).gameObject.name == "apples")
            {
                isAppleTree = true;
                break;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale.x <= MaxSize)
        {
            gameObject.transform.localScale = new Vector3(
                gameObject.transform.localScale.x + Time.deltaTime / timeModifier,
                gameObject.transform.localScale.y + Time.deltaTime / timeModifier,
                gameObject.transform.localScale.z + Time.deltaTime / timeModifier);

            if (isAppleTree && !isFlourishing && gameObject.transform.localScale.x >= 1)
            {
                StartCoroutine(appleCountdown(Random.Range(3f, 10f)));
            }
        } else
        {
            StartCoroutine(deathCountdown(Random.Range(20f, 60f)));
        }

    }

    private IEnumerator deathCountdown (float staletime)
    {
        yield return new WaitForSeconds(staletime);

        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (gameObject.transform.GetChild(i).gameObject.name == "Icosphere" || gameObject.transform.GetChild(i).gameObject.name == "apples")
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            } 
        }


        StartCoroutine(rotCountdown());
    }

    private IEnumerator rotCountdown()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    private IEnumerator appleCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (gameObject.transform.GetChild(i).gameObject.name == "apples")
            {
                gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = applered;
                break;
            }
        }
        
        isFlourishing = true;
    }

    private void deflourish()
    {
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (gameObject.transform.GetChild(i).gameObject.name == "apples")
            {
                gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = transparent;
                break;
            }
        }
        isFlourishing = false;
    }
}
