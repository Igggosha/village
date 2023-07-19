using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  This class is used to generate a noise map using the perlin noise algorithm
    /// </summary>
    public static class PerlinNoiseGenerator
    {
        /// <summary>
        ///  Type of normalization to use when generating the noise map
        ///  If local is selected, the noise map will be normalized for each chunk
        ///  If global is selected, the noise map will be normalized for the whole map
        /// </summary>
        public enum NormalizeMode
        {
            Local,
            Global
        }

        public static float[,] GenerateNoiseMap(int mapSize, NoiseSettings noiseSetting, Vector2 center)
        {
            float[,] noiseMap = new float[mapSize, mapSize];
            Vector2 offset = noiseSetting.offset + center;

            System.Random prng = new System.Random(noiseSetting.seed);

            float maxPossibleHeight = 0;
            float amplitude = 1;
            float frequency = 1;

            Vector2[] octaveOffsets = new Vector2[noiseSetting.octaves];
            for (int i = 0; i < noiseSetting.octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) - offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= noiseSetting.persistance;
            }

            float maxLocalNoiseHeight = float.MinValue;
            float minLocalNoiseHeight = float.MaxValue;

            float halfWidth = mapSize / 2f;
            float halfHeight = mapSize / 2f;


            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    float noiseHeight = 0;
                    amplitude = 1;
                    frequency = 1;

                    for (int i = 0; i < noiseSetting.octaves; i++)
                    {
                        float xCoord = (x - halfWidth + octaveOffsets[i].x) / noiseSetting.scale * frequency;
                        float yCoord = (y - halfHeight + octaveOffsets[i].y) / noiseSetting.scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= noiseSetting.persistance;
                        frequency *= noiseSetting.lacunarity;
                    }

                    if (noiseHeight > maxLocalNoiseHeight)
                    {
                        maxLocalNoiseHeight = noiseHeight;
                    }

                    if (noiseHeight < minLocalNoiseHeight)
                    {
                        minLocalNoiseHeight = noiseHeight;
                    }


                    noiseMap[x, y] = noiseHeight;

                    if (noiseSetting.normalizeMode == NormalizeMode.Global)
                    {
                        float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight);
                        noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                    }

                }
            }

            if (noiseSetting.normalizeMode == NormalizeMode.Local)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    for (int x = 0; x < mapSize; x++)
                    {
                        noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    }
                }
            }

            return noiseMap;
        }
    }

    /// <summary>
    ///  This class is used to store the settings used to generate the noise map
    /// </summary>
    [System.Serializable]
    public class NoiseSettings
    {
        public PerlinNoiseGenerator.NormalizeMode normalizeMode;
        
        public float scale = 50;
        public int octaves = 6;
        [Range(0, 1)] public float persistance = .6f;
        public float lacunarity = 2;
        public int seed;
        public Vector2 offset;

        public void ValidateValues()
        {
            scale = Mathf.Max(scale, 0.01f);
            octaves = Mathf.Max(octaves, 1);
            lacunarity = Mathf.Max(lacunarity, 1);
            persistance = Mathf.Clamp01(persistance);
        }

    }

}