using System;
using Data;
using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    /// Generate a terrain chunk 
    /// </summary>
    public class TerrainChunk
    {
        public event Action<TerrainChunk, bool> onVisibilityChanged;

        public Vector2 coord;

        private Vector2 sampleCenter;
        private GameObject meshObject;
        private Bounds bounds;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;
        private MeshData meshData;

        private bool isInitiated;
        private LODInfo[] detailLevels;
        private LODMesh[] lodMeshes;

        private HeightMap heightMap;
        private bool heightMapReceived;
        private int previousLODIndex = -1;
        private float maxViewDistance;

        private HeightMapSetting heightMapSetting;
        private MeshSettings meshSettings;

        private Transform viewer;

        public TerrainChunk(Vector2 coord, HeightMapSetting heightMapSetting, MeshSettings meshSettings,
            LODInfo[] detailLevels, Transform parent, Transform viewer, Material material)
        {
            this.coord = coord;
            this.heightMapSetting = heightMapSetting;
            this.meshSettings = meshSettings;
            this.viewer = viewer;

            sampleCenter = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
            Vector2 positionV2 = coord * meshSettings.meshWorldSize;
            bounds = new Bounds(positionV2, Vector2.one * meshSettings.meshWorldSize);

            this.detailLevels = detailLevels;

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;

            meshObject.transform.position = new Vector3(positionV2.x, 0, positionV2.y);
            meshObject.transform.parent = parent;

            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            maxViewDistance = detailLevels[^ 1].visibleDistanceThreshold;

        }

        public void Load()
        {
            ThreadedDataRequester.RequestData(
                () => HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, heightMapSetting,
                    sampleCenter), OnHeightReceived);

        }

        void OnHeightReceived(object heightMapObject)
        {
            heightMap = (HeightMap)heightMapObject;
            heightMapReceived = true;

            UpdateTerrainChunk();

        }

        Vector2 viewerPosition
        {
            get { return new Vector2(viewer.position.x, viewer.position.z); }
        }


        public void UpdateTerrainChunk()
        {
            if (!heightMapReceived) return;

            float viewerDistanceFromNearestEdge = (float)Math.Sqrt(bounds.SqrDistance(viewerPosition));
            bool wasVisible = IsVisible();
            bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

            if (visible)
            {
                int lodIndex = 0;

                for (int i = 0; i < detailLevels.Length - 1; i++)
                {
                    if (viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lodIndex != previousLODIndex)
                {
                    LODMesh lodMesh = lodMeshes[lodIndex];
                    if (lodMesh.hasMesh)
                    {
                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                        meshCollider.sharedMesh = lodMesh.mesh;
                    }
                    else if (!lodMesh.hasRequestedMesh)
                    {
                        lodMesh.RequestMesh(heightMap, meshSettings);
                    }
                }


            }

            if (wasVisible == visible) return;
            
            SetVisible(visible);
            if (onVisibilityChanged != null)
            {
                onVisibilityChanged(this, visible);
            }
        }

        public void SetVisible(bool visible)
        {
            if (!isInitiated && meshData != null)
            {
                meshFilter.mesh = meshData.CreateMesh();
                isInitiated = true;
            }

            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
    
    internal class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        private int lod;

        private Action updateCallback;

        public LODMesh(int lod, Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
        {
            hasRequestedMesh = true;
            ThreadedDataRequester.RequestData(
                () => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);
        }

        void OnMeshDataReceived(object meshDataObject)
        {
            mesh = ((MeshData)meshDataObject).CreateMesh();
            hasMesh = true;

            updateCallback?.Invoke();
        }

    }
}