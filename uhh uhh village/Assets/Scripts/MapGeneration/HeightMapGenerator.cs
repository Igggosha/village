using Data;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  This class is used to generate  a height map 
    /// </summary>
    public static class HeightMapGenerator
    {
        public static HeightMap GenerateHeightMap(int size, HeightMapSetting heightMapSetting, Vector2 sampleCenter)
        {
            float[,] values = PerlinNoiseGenerator.GenerateNoiseMap(size, heightMapSetting.noiseSettings, sampleCenter);
        
            AnimationCurve heightCurveThreadsafe = new AnimationCurve(heightMapSetting.heightCurve.keys); 
        
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;
        
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size ; j++)
                {
                    values[i, j] *= heightCurveThreadsafe.Evaluate( values[i, j]) * heightMapSetting.heightMultiplier;
                
                    if (values[i, j] > maxValue)
                    {
                        maxValue = values[i, j];
                    }
                
                    if(values[i,j] < minValue)
                    {
                        minValue = values[i, j];
                    }
                }
            }
        
            return new HeightMap(values, minValue, maxValue); 
        }
    }

    /// <summary>
    /// This struct is used to store the height map values
    /// </summary>
    public struct HeightMap
    {
        public readonly float[,] values;
        public readonly float minValue;
        public readonly float maxValue;


        public HeightMap(float[,] values, float minValue, float maxValue)
        {
            this.values = values;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}