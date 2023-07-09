using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI ;
using UnityEngine.UI;

/// <summary>
///     Villager AI.
///
///
///
///     To be assigned to villagers.
///
///     TODO: a lot
/// </summary>
public class VillagerBehaviour : MonoBehaviour
{
    /*
     
     </summary>
     */
    private string[] splashesList = {
        "I've got a lot on my mind tonight.",
        "I wish pizza was a thing in my world.",
        "Why aren't there any hedgehogs in here?",
        "Sometimes, I forget my parent's names. Did I even have parents?",
        "You stink. Me too. We stink.",
        "Eating apples every day is so monogamous... I'm thinking of eating someone.",
        "*burp* what?",
        "I have heard legends of horrible scary bears but never seen them.",
        "Sometimes I get uncontrollable urges and I don't know what to do.",
        "I want to eat a chicken. Too bad we don't have chicken.",
        "I wonder if those bear things are tasty.",
        "Mine is bigger.",
        "You look like a child of a bear and a cow.",
        "Why do trees grow?",
        "I'm glad gods didn't add taxes here. Hell, those legends are terrifying.",
        "You are charming. Just joking, you make me want to become infertile.",
        "I want to uhh, uhh.. uhh... Ah! wait... um... Oh yeah!",
        "Me thinking what to do.."};

    public Gender gender = Gender.female;

    public enum Gender
    {
        female,
        male
    }

    public string villagerName = "";

    private string[] femaleNames = {
        "Maria",
        "Johanna",
        "Athena"
    };

    private string[] maleNames = {
        "Martin",
        "Walter",
        "John"
    };

    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    public Text text;

    [SerializeField][Range(0f,100f)] public float hunger = 100;

    [SerializeField][Range(0,4)] public int priority = 0;


    private string[] priorityValues = { "gather food for the village", "get shelter", "get enough food for myself", "find a mate and mate", "explore the world and science" };

    public GameObject dialogGUI;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // TODO make a detection of gender based on gameObject name or model

        if (gender == Gender.female)
        {
            string n0 = femaleNames[Random.Range(0, femaleNames.Length)];
            villagerName = n0 + " " + femaleNames[Random.Range(0, femaleNames.Length)];
        } else if (gender == Gender.male)
        {
            string n0 = maleNames[Random.Range(0, maleNames.Length)];
            villagerName = n0 + " " + maleNames[Random.Range(0, maleNames.Length)];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnMouseDown()
    {
        if (!dialogGUI.activeSelf)
        {
            displayThoughts();
        }
    }

    /// <summary>
    /// This function is to be called when the villager is clicked to show their
    /// thoughts - their top priority, their current task, (maybe) their mood and
    /// a splash text.
    /// </summary>
    private void displayThoughts()
    {
        dialogGUI.SetActive(true);

        for (int i = 0; i < dialogGUI.transform.childCount; i++)
        {
            if (dialogGUI.transform.GetChild(i).gameObject.name == "ThoughtText")
            {
                string thought = "\"" + splashesList[Random.Range(0, splashesList.Length)] + "\"\nMy top priority now is to " + priorityValues[priority] + "."; 

                dialogGUI.transform.GetChild(i).gameObject.GetComponent<Text>().text = thought;
                continue;
            } else if (dialogGUI.transform.GetChild(i).gameObject.name == "VillagerName")
            {
                dialogGUI.transform.GetChild(i).gameObject.GetComponent<Text>().text = villagerName;
            }
        }
    }
}
