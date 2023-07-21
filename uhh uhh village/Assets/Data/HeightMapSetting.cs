using MapGeneration;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Settings for the height map generation
    /// </summary>
    [CreateAssetMenu()]
    public class HeightMapSetting : UpdatableData
    {
        public NoiseSettings noiseSettings;
        
        public float heightMultiplier;
        public AnimationCurve heightCurve;

        public float minHeight
        {
            get { return  heightMultiplier * heightCurve.Evaluate(0); }
        }
        
        public float maxHeight
        {
            get { return  heightMultiplier * heightCurve.Evaluate(1); }
        } 
    
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            noiseSettings.ValidateValues(); 
            base.OnValidate(); 
        }
#endif 
    }
}
