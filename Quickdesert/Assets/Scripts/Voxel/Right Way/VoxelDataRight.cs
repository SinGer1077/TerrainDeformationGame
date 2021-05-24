using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelDataRight 
{
    
    public int[,,] data = new int[1,2,3];
    public List<Vector3> vertices;
    public List<int> triangles;

    public int X
    {
        get { return data.GetLength(0); }
    }
    public int Y
    {
        get { return data.GetLength(1); }
    }
    public int Z
    {
        get { return data.GetLength(2); }
    }
    public VoxelDataRight(int [,,] voxelMesh)
    {
        data = voxelMesh;
    }
    public int GetCell(int x, int y, int z)
    {
        return data[x, y, z];
    }
    public int GetNeighbor(int x, int y, int z, Direction dir)
    {
        DataCoordinate offsetToCheck = offsets[(int)dir];
        DataCoordinate neighborCoord = new DataCoordinate(x + offsetToCheck.x,
            y + offsetToCheck.y, z + offsetToCheck.z);

        if (neighborCoord.x < 0 || neighborCoord.x >= X || 
            neighborCoord.y < 0 || neighborCoord.y >= Y ||
            neighborCoord.z < 0 || neighborCoord.z >= Z)        
            return 0;        
        else        
            return GetCell(neighborCoord.x, neighborCoord.y, neighborCoord.z);
        
    }
    public void GenerateVoxelMesh(float scale, float adjScale)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y <Y; y++)
            {
                for (int z = 0; z < Z; z++)
                {
                    if (GetCell(x, y, z) == 0)
                        continue;
                    MakeCube(adjScale, new Vector3(x * scale, y * scale, z * scale), x, y, z);
                }
            }
        }
    }

    private void MakeCube(float cubeScale, Vector3 cubePos, int x, int y, int z)
    {
        for (int i = 0; i < 6; i++)
        {
            if (GetNeighbor(x, y, z, (Direction)i) == 0)
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
    struct DataCoordinate
    {
        public int x;
        public int y;
        public int z;
        public DataCoordinate (int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    DataCoordinate[] offsets =
    {
        new DataCoordinate(0, 0, 1),
        new DataCoordinate(1, 0, 0),
        new DataCoordinate(0, 0, -1),
        new DataCoordinate(-1, 0, 0),
        new DataCoordinate(0, 1, 0),
        new DataCoordinate(0, -1, 0)
    };
}
public enum Direction
{
    North,
    East, 
    South,
    West,
    Up,
    Down
}
