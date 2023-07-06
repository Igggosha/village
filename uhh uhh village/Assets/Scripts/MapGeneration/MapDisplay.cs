using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  Class to display the map in the scene
    ///  Draws the map as a texture or as a mesh
    /// 
    /// TODO: This class wiil be refactored in the future
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class MapDisplay : MonoBehaviour
    {
        // reference to the mesh filter and mesh renderer components
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;

        // reference to the texture renderer used to draw the texture
        private Renderer textureRenderer;

        public void DrawTexture(Texture2D texture)
        {
            textureRenderer = GetComponent<Renderer>();
            textureRenderer.sharedMaterial.mainTexture = texture;
            textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }

        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }

    /// <summary>
    /// Struct to hold the terrain type data for the map generator
    /// </summary>
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}