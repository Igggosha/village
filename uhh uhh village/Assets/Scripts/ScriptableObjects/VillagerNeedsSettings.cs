using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/VillagerNeedsSettings")]
    public class VillagerNeedsSettings : ScriptableObject
    {
        public float foodDecreaseRate = 0.1f;
        public float energyDecreaseRate = 0.1f;
        public float moodDecreaseRate = 0.1f;
        public float socialDecreaseRate = 0.1f;
    }
}