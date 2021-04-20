using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderRight : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    public float scale = 1f;
    float adjScale;

    public int mapWidth;
    public int mapHeight;
    public float mapScale;

    VoxelDataRight voxelData;
    Collider coll;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
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
        voxelData = new VoxelDataRight(data);
        //GenerateWithPrimitives(voxelData);
        GenerateVoxelMesh(voxelData);
        UpdateMesh();
        coll = GetComponent<Collider>();
    }
    private void Update()
    {
        ClickOnMesh();        
    }
    private void ClickOnMesh()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            
            RaycastHit hit;
            if (coll.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.point);
                Vector3 clickedBlockCoordinates = GetClickedBlockCoordinates(hit.point);
                DeleteVoxel(clickedBlockCoordinates);
                GenerateVoxelMesh(voxelData);
                UpdateMesh();
            }
        }
    }
    private Vector3 GetClickedBlockCoordinates(Vector3 hitPoint)
    {
        int x = (int)(hitPoint.x - (hitPoint.x % 1)); 
        int y = (int)(hitPoint.y - (hitPoint.y % 1));
        int z = (int)(hitPoint.z - (hitPoint.z % 1));
        
        //correct x
        if (Camera.main.transform.position.x < x)
        {
            if (Mathf.Floor((hitPoint.x % 1) * 10) >= 5 && x < voxelData.data.GetLength(0) - 1)
                x++;
        }
        else
            if (Mathf.Floor((hitPoint.x % 1) * 10) > 5 && x < voxelData.data.GetLength(0) - 1)
                x++;

        //correct y
        if (Camera.main.transform.position.y < y)
        {
            if (Mathf.Floor((hitPoint.y % 1) * 10) >= 5 && y < voxelData.data.GetLength(2) - 1)
                y++;
        }
        else
             if (Mathf.Floor((hitPoint.y % 1) * 10) > 5 && y < voxelData.data.GetLength(2) - 1)
                y++;

        //correct z
        if (Camera.main.transform.position.z < z)
        {
            if (Mathf.Floor((-hitPoint.z % 1) * 10) > 5 && -z < voxelData.data.GetLength(1) - 1)
                z--;
        }
        else
            if (Mathf.Floor((-hitPoint.z % 1) * 10) >= 5 && -z < voxelData.data.GetLength(1) - 1)
                z--;

        Debug.Log(x.ToString() + " " + y.ToString() + " "  + z.ToString());

        Vector3 block = new Vector3(x, z, y);
        return block;
    }
    private void DeleteVoxel(Vector3 block)
    {
        if (voxelData.GetCell((int)block.x, -(int)block.y, (int)block.z) != 0)
            voxelData.data[(int)block.x, -(int)block.y, (int)block.z] = 0;
    }

    private int[,,] BuildMesh(ref int[,,] data, float[,] heightMap)
    {
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                int height = (int)(heightMap[x, y] * 10);
                while (height >= 0)
                {
                    data[x, y, height] = 1;
                    height--;
                }
            }
        }        
        return data;
    }
    private void GenerateWithPrimitives(VoxelDataRight data)
    {
        for (int x = 0; x < data.X; x++)
        {
            for (int y = 0; y < data.Y; y++)
            {
                for (int z = 0; z < data.Z; z++)
                {
                    if (data.GetCell(x, y, z) == 0)
                        continue;
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x, z, y);
                }
            }
        }
    }

    private void GenerateVoxelMesh1(VoxelDataRight data)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        for (int z = 0; z < data.Z; z++)
        {
            for (int x = 0; x < data.X; x++)
            {
                for (int y = 0; y < data.Y; y++)
                {
                    if (data.GetCell(x, z, y) == 0)
                        continue;
                    MakeCube(adjScale, new Vector3(x * scale, y * scale, z * scale), x, z, y, data);
                }
            }
        }
    }
    private void GenerateVoxelMesh(VoxelDataRight data)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        for (int x = 0; x < data.X; x++)
        {
            for (int y = 0; y < data.Y; y++)
            {
                for (int z = 0; z < data.Z; z++)
                {
                    if (data.GetCell(x, y, z) == 0)
                        continue;
                    MakeCube(adjScale, new Vector3(x * scale, y * scale, z * scale), x, y, z, data);
                }
            }
        }
    }

    private void MakeCube(float cubeScale, Vector3 cubePos, int x, int y, int z, VoxelDataRight data)
    {
        for (int i = 0; i < 6; i++)
        {
            if (data.GetNeighbor(x, y, z, (Direction)i) == 0)
                MakeFace((Direction)i, cubeScale, cubePos);            
        }
    }
    private void MakeFace(Direction dir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(CubeMeshDataRight.faceVertices(dir, faceScale, facePos));
        int vCount = vertices.Count;

        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);
    }
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        MeshCollider meshCol = GetComponent<MeshCollider>();
        meshCol.sharedMesh = mesh;
    }
}
