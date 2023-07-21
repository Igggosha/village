using Data;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  Class to display the map in the scene
    ///  Draws the map as a texture or as a mesh 
    /// 
    /// TODO: This class wiil be refactored in the future
    /// </summary>
    public class MapPreview : MonoBehaviour
    {
        [SerializeField] DrawMode drawMode;

        public MeshSettings meshSettings;
        public HeightMapSetting heightMapSetting;
        public TextureData textureData;

        public Material terrainMaterial;

        /// <summary>
        ///  Draw mode enum for drawing noise map, color map or mesh
        /// </summary>
        private enum DrawMode
        {
            NoiseMap,
            Mesh,
            FalloffMap
        }

        /// <summary>
        /// Level of detail of the map chunk (1 - 6)
        ///  1 - 241x241 pixels 2 - 121x121 pixels 3 - 61x61 pixels 4 - 31x31 pixels 5 - 16x16 pixels 6 - 8x8 pixels
        /// </summary>
        [Range(0, MeshSettings.numSupportedLODs - 1)] [SerializeField]
        private int editorPrevioLevelOfDetail;

        [SerializeField] public bool autoUpdate;

        // reference to the mesh filter and mesh renderer components
        [SerializeField] private MeshFilter meshFilter;

        // reference to the texture renderer used to draw the texture
        public Renderer textureRenderer;

        public void DrawTexture(Texture2D texture)
        {
            textureRenderer.sharedMaterial.mainTexture = texture;
            textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

            textureRenderer.gameObject.SetActive(true);
            meshFilter.gameObject.SetActive(false);
        }

        public void DrawMesh(MeshData meshData)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();

            textureRenderer.gameObject.SetActive(false);
            meshFilter.gameObject.SetActive(true);
        }

        public void DrawMapInEditor()
        {
            textureData.ApplyToMaterial(terrainMaterial);
            textureData.UpdateMeshHeights(terrainMaterial, heightMapSetting.minHeight, heightMapSetting.maxHeight);

            HeightMap heightMap =
                HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, heightMapSetting, Vector2.zero);


            if (drawMode == DrawMode.NoiseMap)
            {
                DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
            }
            else if (drawMode == DrawMode.Mesh)
            {
                DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPrevioLevelOfDetail));
            }
            else if (drawMode == DrawMode.FalloffMap)
            {
                /*DrawTexture(TextureGenerator.TextureFromHeightMap(
                    new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), 0, 1)));
                    */
                
                DrawMesh(MeshGenerator.GenerateTerrainMesh(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), meshSettings, editorPrevioLevelOfDetail));
            }

        }
        
        void OnTextureValuesUpdated()
        {
            textureData.ApplyToMaterial(terrainMaterial);
        }

        void OnValuesUpdated()
        {
            textureData.ApplyToMaterial(terrainMaterial);

            if (!Application.isPlaying)
            {
                DrawMapInEditor();
            }
        }
        

        // Called when a value is changed in the inspector
        private void OnValidate()
        {
            if (meshSettings != null)
            {
                meshSettings.OnValuesUpdated -= OnValuesUpdated;
                meshSettings.OnValuesUpdated += OnValuesUpdated;
            }

            if (heightMapSetting != null)
            {
                heightMapSetting.OnValuesUpdated -= OnValuesUpdated;
                heightMapSetting.OnValuesUpdated += OnValuesUpdated;
            }

            if (textureData != null)
            {
                textureData.OnValuesUpdated -= OnTextureValuesUpdated;
                textureData.OnValuesUpdated += OnTextureValuesUpdated;
            }
        }
        
    }
}