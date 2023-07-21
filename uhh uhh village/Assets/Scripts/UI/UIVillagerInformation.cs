using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Villanger;

namespace UI
{
    /// <summary>
    ///  This class is responsible for showing the information about the villager.
    /// </summary>
    public class UIVillagerInformation : MonoBehaviour
    {

        [SerializeField] private Image foodBar;
        [SerializeField] private Image energyBar;
        [SerializeField] private Image moodBar;
        [SerializeField] private Image socialBar;
    
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI thoughtsText;
    
        [SerializeField] private GameObject maleIcon;
        [SerializeField] private GameObject femaleIcon;

        
        private VillagerBehaviour _villager;
        
        public void InitializePanel(VillagerBehaviour villager)
        {
            _villager = villager;
            
            nameText.text = villager.GetName();
            ageText.text = villager.GetAge() + " yo.";
            thoughtsText.text = VillagerConst.SplashesList[Random.Range(0, VillagerConst.SplashesList.Length)];
            
            maleIcon.SetActive(villager.GetGender() == VillagerBehaviour.Gender.male);
            femaleIcon.SetActive(villager.GetGender() == VillagerBehaviour.Gender.female);
            
            UpdateNeeds(villager.GetVillagerNeeds());
            
            villager.GetVillagerNeeds().OnNeedsChanged += UpdateNeeds;
          
        }
        
        private void UpdateNeeds(VillagerNeeds needs)
        {
            foodBar.fillAmount = needs.GetFoodValue() / needs.GetMaxValueOfNeeds();
            energyBar.fillAmount = needs.GetEnergyValue() / needs.GetMaxValueOfNeeds();
            moodBar.fillAmount = needs.GetMoodValue() / needs.GetMaxValueOfNeeds();
            socialBar.fillAmount = needs.GetSocialValue() / needs.GetMaxValueOfNeeds();
        }
        
        public void ClosePanel()
        {
            if(transform.gameObject.activeSelf == false) return;
            
            _villager.GetVillagerNeeds().OnNeedsChanged -= UpdateNeeds;
            gameObject.SetActive(false);
        }

    }
}
