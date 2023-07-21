using UnityEngine;

namespace UI
{
    /// <summary>
    ///   This class is responsible for controlling the whole UI.
    /// </summary>
    public class UIController : MonoBehaviour
    {
       [SerializeField] private UIVillagerInformation uiVillagerInformation;

        public static UIController Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        
        public void ShowVillagerInformationPanel(VillagerBehaviour villager)
        {
            uiVillagerInformation.gameObject.SetActive(true);
            uiVillagerInformation.InitializePanel(villager);
        }
        
        
        public void OnMissTap()
        {
            uiVillagerInformation.ClosePanel();
        }
    }
}