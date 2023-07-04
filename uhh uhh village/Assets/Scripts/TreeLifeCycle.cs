using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLifeCycle : MonoBehaviour
{
    //[SerializeField] private int MaxLifeTime;
    [SerializeField] private int MaxSize = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale.x <= MaxSize)
        {
            gameObject.transform.localScale = new Vector3(
                gameObject.transform.localScale.x + Time.deltaTime / 10,
                gameObject.transform.localScale.y + Time.deltaTime / 10,
                gameObject.transform.localScale.z + Time.deltaTime / 10);
        } else
        {
            StartCoroutine(deathCountdown());
        }

    }

    private IEnumerator deathCountdown ()
    {
        yield return new WaitForSeconds(20);

        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (gameObject.transform.GetChild(i).gameObject.name == "Icosphere")
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
}
