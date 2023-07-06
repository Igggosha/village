using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  Map generator class to generate the map chunk and display it in the scene
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {

        [SerializeField] DrawMode drawMode;

        /// <summary>
        ///  Size of the map chunk in pixels
        /// </summary>
        private const int mapChunkSize = 241;
        
        //TODO: Going to be added in the future
        [SerializeField] private int numberOfChunks;
        
        [SerializeField] private float noiseScale;

        /// <summary>
        /// Level of detail of the map chunk (1 - 6)
        ///  1 - 241x241 pixels 2 - 121x121 pixels 3 - 61x61 pixels 4 - 31x31 pixels 5 - 16x16 pixels 6 - 8x8 pixels
        /// </summary>
        [Range(1, 6)] [SerializeField] private int levelOfDetail;

        [SerializeField] private int octaves;
        [SerializeField] [Range(0, 1)] private float persistance;
        [SerializeField] private float lacunarity;
        [SerializeField] private Vector2 offset;

        [SerializeField] private int seed;

        [SerializeField] private float meshHeightMultiplier;
        [SerializeField] private AnimationCurve meshHeightCurve;
        
        [SerializeField] public bool autoUpdate;

        [SerializeField] private TerrainType[] regions;

        /// <summary>
        ///  Draw mode enum for drawing noise map, color map or mesh
        /// </summary>
        private enum DrawMode
        {
            NoiseMap,
            ColorMap,
            Mesh
        }


        private void Awake()
        {
            GenerateNoiseMap();
        }

        public void GenerateNoiseMap()
        {
            Point mapChunkSize = new Point(MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);

            // Generate noise map using Perlin noise algorithm
            float[,] noiseMap = PerlinNoiseGenerator.GenerateNoise(mapChunkSize, seed, noiseScale, octaves, persistance,
                lacunarity, offset);

            // Generate color map based on noise map values and regions 
            Color[] colorMap = new Color[mapChunkSize.x * mapChunkSize.y];
            for (int y = 0; y < mapChunkSize.y; y++)
            {
                for (int x = 0; x < mapChunkSize.x; x++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight <= regions[i].height)
                        {
                            colorMap[y * mapChunkSize.x + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }

            // Draw map based on draw mode
            // TODO: Going to be deprecated 
            MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

            if (drawMode == DrawMode.ColorMap)
            {
                mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize));
            }
            else if (drawMode == DrawMode.NoiseMap)
            {
                mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            }
            else if (drawMode == DrawMode.Mesh)
            {
                mapDisplay.DrawMesh(
                    MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),
                    TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize));
            }
        }


        // Called when a value is changed in the inspector
        private void OnValidate()
        {
            if (lacunarity < 1)
            {
                lacunarity = 1;
            }

            if (octaves < 0)
            {
                octaves = 0;
            }
        }
    }
}