using System.Collections.Generic;
using Data;
using UnityEngine;

namespace MapGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        private const float viewerMoveThresholdForChunkUpdate = 25f;

        private const float sqrViewerMoveThresholdForChunkUpdate =
            viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

        [SerializeField] private LODInfo[] detailLevels;

        [SerializeField] private MeshSettings meshSettings;
        [SerializeField] private HeightMapSetting heightMapSetting;
        [SerializeField] private TextureData textureData;

        [SerializeField] private Transform viewer;
        [SerializeField] private Material mapMaterial;

        private Vector2 viewerPositionOld;
        private Vector2 viewerPosition;
        private float meshWorldChunkSize;
        private int chunksVisibleInViewDistance;

        private Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
        private List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

        private void Start()
        {
            textureData.ApplyToMaterial(mapMaterial);
            textureData.UpdateMeshHeights(mapMaterial, heightMapSetting.minHeight, heightMapSetting.maxHeight);


            meshWorldChunkSize = meshSettings.meshWorldSize;
            float maxViewDistance = detailLevels[^1].visibleDistanceThreshold;
            chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / meshWorldChunkSize);

            UpdateVisibleChunks();
        }

        public void Update()
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

            if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
            {
                viewerPositionOld = viewerPosition;
                UpdateVisibleChunks();
            }
        }

        private void UpdateVisibleChunks()
        {
            HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();

            for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
            {
                alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
                visibleTerrainChunks[i].UpdateTerrainChunk();
            }


            int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldChunkSize);
            int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldChunkSize);

            for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++)
            {
                for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++)
                {
                    Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                    if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
                    {
                        if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                        {
                            terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();

                        }
                        else
                        {
                            TerrainChunk chunk = new TerrainChunk(viewedChunkCoord, heightMapSetting, meshSettings,
                                detailLevels, transform, viewer, mapMaterial);
                            terrainChunkDictionary.Add(viewedChunkCoord, chunk);
                            chunk.onVisibilityChanged += OnTerrainVisibilityChanged;
                            chunk.Load();
                        }
                    }
                }
            }
        }

        void OnTerrainVisibilityChanged(TerrainChunk chunk, bool isVisible)
        {
            if (isVisible)
            {
                visibleTerrainChunks.Add(chunk);
            }
            else
            {
                visibleTerrainChunks.Remove(chunk);
            }
        }

    }

    [System.Serializable]
    public struct LODInfo
    {
        [Range(0, MeshSettings.numSupportedLODs - 1)]
        public int lod;

        public float visibleDistanceThreshold;

    }
}