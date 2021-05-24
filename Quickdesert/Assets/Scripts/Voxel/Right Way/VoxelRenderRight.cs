using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderRight : MonoBehaviour
{    
    public float scale = 1f;
    float adjScale;

    public int mapWidth;
    public int mapHeight;
    public float mapScale;

    public VoxelDataStorage voxelDataStorage;
   
    Collider coll;
    MeshCollider meshCol;

    private void Awake()
    {
        voxelDataStorage.mesh = GetComponent<MeshFilter>().mesh;
        adjScale = scale * 0.5f;
    }

    private void Start()
    {
        TerrainGenerator generator = new TerrainGenerator();
        float[,] heightMap = new float[mapWidth,mapHeight];
        float max = -1;
        generator.GetMaxHeight(mapWidth, mapHeight, mapScale, ref heightMap, ref max);

        int[,,] data = new int[mapWidth, mapHeight, (int)(max*10)+1];
        data = BuildMesh(ref data, heightMap);
        voxelDataStorage.voxelData = new VoxelDataRight(data);
        //GenerateWithPrimitives(voxelData);
        voxelDataStorage.voxelData.GenerateVoxelMesh(scale, adjScale);
        coll = GetComponent<Collider>();
        meshCol = GetComponent<MeshCollider>();
        voxelDataStorage.UpdateMesh();
    }
    private void Update()
    {       
        //meshCol.sharedMesh = voxelDataStorage.mesh;
    }    
    /// <summary>
    /// заполняем воксельную 3д сетку
    /// </summary>
    /// <param name="data"></param>
    /// <param name="heightMap"></param>
    /// <returns></returns>
    private int[,,] BuildMesh(ref int[,,] data, float[,] heightMap)
    {
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                //конвертируем значение высоты в количество вокселей
                int height = (int)(heightMap[x, y] * 10);
                //заполняем матрицу по оси Z
                while (height >= 0)
                {
                    data[x, y, height] = 1;
                    height--;
                }
            }
        }        
        return data;
    }
    
    
}
