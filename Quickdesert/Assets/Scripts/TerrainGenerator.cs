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
    /// <summary>
    /// генерация карты высот
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
   float [,] GenerateHeights(int width, int height, float scale)
    {
        float[,] heights = new float[width, height];        
        for (int x = 0; x<width; x++)
        {
            for (int y = 0; y<height; y++)
            {
                //для каждой ячейки вычисляем высоту
                float value = CalculateHeight(x, y, scale, width, height);
                heights[x, y] = value;
            }
        }
        return heights;
    }
    /// <summary>
    /// вычисление высоты
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="scale"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    float CalculateHeight(float x, float y, float scale, int width, int height)
    {
        //получаем координаты для плавного перехода
        float xCoord = x / width * scale;
        float yCoord = y / height * scale;
        //возвращаем полученное значение от 0.0 до 1.0
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
