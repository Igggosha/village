using UI;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 5;
    private Vector3 ObjectPosition;


    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }

    [Range(0.1f, 9f)] [SerializeField] float sensitivity = 2f;

    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)]
    [SerializeField]
    float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;

    const string
        xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage

    const string yAxis = "Mouse Y";



    // Start is called before the first frame update
    void Start()
    {
        ObjectPosition = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Camera.main.transform.forward * (MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Camera.main.transform.right * (MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Camera.main.transform.forward * (MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Camera.main.transform.right * (MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Camera.main.transform.up * (MovementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= Camera.main.transform.up * (MovementSpeed * Time.deltaTime);
        }




        if (Input.GetMouseButton(1))
        {
            rotation.x += Input.GetAxis(xAxis) * sensitivity;
            rotation.y += Input.GetAxis(yAxis) * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

            transform.localRotation = xQuat * yQuat;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Villager"))
                {
                    UIController.Instance.ShowVillagerInformationPanel(hit.transform.GetComponent<VillagerBehaviour>());
                }
                else
                {
                    UIController.Instance.OnMissTap();
                }
            }
        }
    }
}