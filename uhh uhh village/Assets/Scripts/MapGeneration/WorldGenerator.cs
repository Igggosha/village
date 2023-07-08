using System;
using UnityEngine;

namespace MapGeneration
{
    //TODO: its going to be refactored in the future 
    public class WorldGenerator : MonoBehaviour
    {

   
        [SerializeField] private GameObject terratin;
        [SerializeField] private string seed;

        [Range(0,100)]
        [SerializeField] private int randomFeelPrecisionTree;
        
        [Range(0,10)]
        [SerializeField] private int smoothIterationsTree;
        
        [Range(0,100)]
        [SerializeField] private int randomFeelPrecisionRock;
        
        [Range(0,10)]
        [SerializeField] private int smoothIterationsRock;

        private Point mapSize;
        private int[,] map;
        private void Awake()
        {
            // If seed is empty, generate random seed
            if (string.IsNullOrEmpty(seed))
            {
                seed = DateTime.Now.Ticks.ToString();
            }

            mapSize = new Point((int)terratin.transform.localScale.x, (int)terratin.transform.localScale.z);
            map = new int[mapSize.x, mapSize.y];
            GenerateMap();

        }


 

        private void GenerateMap()
        {
            MapGeneratorSource mapGeneratorSource = new MapGeneratorSource();
            bool[,] mapTrees =  mapGeneratorSource.GenerateMap(mapSize, randomFeelPrecisionTree, smoothIterationsTree, seed);
            bool[,] mapRocks = mapGeneratorSource.GenerateMap(mapSize, randomFeelPrecisionRock, smoothIterationsRock, seed);
            
            for ( int x = 0; x < map.GetLength(0); x++)
            {
                for ( int y = 0; y < map.GetLength(1); y++)
                {
                    int value = 0;
                    if (!mapRocks[x, y]) value = 1;
                    else if (!mapTrees[x, y]) value = 2;

                    map[x,y] = value;
                }
            }
            
         
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                seed = DateTime.Now.Ticks.ToString();
                GenerateMap();
                print( "Map generated");
            }
        }

        private void OnDrawGizmos()
        {
            if (map == null) return;
            
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Color color = map[x, y] switch
                    {
                        0 => Color.black,
                        1 => Color.gray,
                        2 => Color.green,
                        _ => Color.red
                    };

                    Gizmos.color = color;
                    Vector3 pos = new Vector3(-terratin.transform.localScale.x / 2 + x + 0.5f, 0, -terratin.transform.localScale.z / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
        
        
        
    }
    
    
}