using UnityEngine;

namespace MapGeneration
{
    /// <summary>
    ///  Map generator class to generate textures from the noise map
    /// </summary> 
    public static class TextureGenerator
    {
        public static Texture2D TextureFromColorMap(Color[] colorMap, Point mapSize)
        {
            Texture2D texture = new Texture2D(mapSize.x, mapSize.y);
            texture.filterMode = FilterMode.Point; 
            texture.wrapMode = TextureWrapMode.Clamp;   
            texture.SetPixels(colorMap);
            texture.Apply();

            return texture;
        }

        public static Texture2D TextureFromHeightMap(float[,] heightMap)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);

            Color[] colorMap = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                }
            }

            return TextureFromColorMap(colorMap, new Point(width, height));
        }
    }
}  