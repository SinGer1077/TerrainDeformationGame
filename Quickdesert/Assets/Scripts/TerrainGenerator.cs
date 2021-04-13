using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int depth;
    public int width;
    public int height;

    public float scale = 20f;

    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }
    float [,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x<width; x++)
        {
            for (int y = 0; y<height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }
    float CalculateHeight(float x, float y)
    {
        float xCoord = x / width * scale;
        float yCoord = y / height * scale;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
