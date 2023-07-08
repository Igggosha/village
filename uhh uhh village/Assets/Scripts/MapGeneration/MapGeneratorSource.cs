using System;

namespace MapGeneration
{
    //TODO: This class is going to be refactored in the future 
    public class MapGeneratorSource
    {
        private bool[,] map;
        private int randomFeelPrecision;
        private string seed;

        public bool[,] GenerateMap(Point mapSize, int randomFeelPrecision, int smoothIterations, string seed)
        {
            this.randomFeelPrecision = randomFeelPrecision;
            this.seed = seed;
            map = new bool[mapSize.x, mapSize.y];
            RandomFillMap();

            for (int i = 0; i < smoothIterations; i++)
            {
                SmoothMap();
            }

            return map;
        }

        private void RandomFillMap()
        {
            Random pseudoRandom = new Random(seed.GetHashCode());

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (x == 0 || y == 0 || x == map.GetLength(0) - 1 || y == map.GetLength(1) - 1)
                    {
                        map[x, y] = true;
                        continue;
                    }


                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFeelPrecision);
                }
            }
        }

        private void SmoothMap()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    int neighbourWallTiles = GetSurroundingGridWalls(x, y);

                    if (neighbourWallTiles > 4)
                    {
                        map[x, y] = true;
                    }
                    else if (neighbourWallTiles < 4)
                    {
                        map[x, y] = false;
                    }
                }
            }

        }

        private int GetSurroundingGridWalls(int x, int y)
        {
            int wallCount = 0;
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < map.GetLength(0) && neighbourY >= 0 &&
                        neighbourY < map.GetLength(1))
                    {
                        if (neighbourX != x || neighbourY != y)
                        {
                            wallCount += map[neighbourX, neighbourY] ? 1 : 0;
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }


    }
}