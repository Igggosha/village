  using UnityEngine;

  /// <summary>
  /// Using only for the editor to hide the game object on play. 
  /// </summary>
public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

}
