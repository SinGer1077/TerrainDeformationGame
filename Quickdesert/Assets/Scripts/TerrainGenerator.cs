using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int depth;
    //public int width;
    //public int height;

    public float scale = 20f;

    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        //terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    //TerrainData GenerateTerrain(TerrainData terrainData)
    //{
    //    //terrainData.heightmapResolution = width + 1;
    //    //terrainData.size = new Vector3(width, depth, height);
    //    //terrainData.SetHeights(0, 0, GenerateHeights(width, height, scale));
    //    //return terrainData;
    //}
   float [,] GenerateHeights(int width, int height, float scale)
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x<width; x++)
        {
            for (int y = 0; y<height; y++)
            {
                float value = CalculateHeight(x, y, scale, width, height);
                heights[x, y] = value;
            }
        }
        return heights;
    }
    float CalculateHeight(float x, float y, float scale, int width, int height)
    {
        float xCoord = x / width * scale;
        float yCoord = y / height * scale;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
    public void GetMaxHeight(int width, int height, float scale, ref float[,] heights,  ref float max)
    {
        heights = GenerateHeights(width, height, scale);        
        for (int i=0; i<heights.GetLength(0); i++)
        {
            for (int j = 0; j<heights.GetLength(1); j++)
            {
                if (max < heights[i, j])
                    max = heights[i,j];
            }
        }        
    }
}
