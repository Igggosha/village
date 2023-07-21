using UnityEngine;

namespace Data
{
    /// <summary>
    /// Settings for the mesh generation 
    /// </summary>
    [CreateAssetMenu()]
    public class MeshSettings : UpdatableData
    {

        public const int numSupportedLODs = 5;

        private const int numSupportedChunkSizes = 9;
        private const int numSupportedFlatShadedChunkSizes = 3;
        private static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

        public float meshScale = 2.5f;
        public bool useFlatShading;

        [Range(0, numSupportedChunkSizes - 1)]
        public int chunkSizeIndex;

        [Range(0, numSupportedFlatShadedChunkSizes - 1)]
        public int flatshadedChunkSizeIndex;

        public int numVertsPerLine
        {
            get { return supportedChunkSizes[(useFlatShading) ? flatshadedChunkSizeIndex : chunkSizeIndex] + 5; }
        }

        public float meshWorldSize
        {
            get { return (numVertsPerLine - 3) * meshScale; }

        }
    }

}