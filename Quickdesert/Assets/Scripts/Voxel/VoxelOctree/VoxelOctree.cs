using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoxelOctree : MonoBehaviour
{
    public float size;
    public int depth = 5;

    VoxelDataRight voxelData;
    public int mapWidth;
    public int mapHeight;
    public float mapScale;
    public Octree<int> octree;

    public bool PaintOnlyEmpty;

    //Mesh mesh;
    //List<Vector3> vertices;
    //List<int> triangles;
    public float scale = 1f;

    public Material material;
    float adjScale;
    // Start is called before the first frame update
    void Start()
    {
        TerrainGenerator generator = new TerrainGenerator();
        float[,] heightMap = new float[mapWidth, mapHeight];
        float max = -1;
        generator.GetMaxHeight(mapWidth, mapHeight, mapScale, ref heightMap, ref max);

        int[,,] data = new int[mapWidth, mapHeight, (int)(max * 10) + 1];
        data = BuildMesh(ref data, heightMap);
        voxelData = new VoxelDataRight(data);

        octree = new Octree<int>(this.transform.position, size, depth, voxelData);

        //vertices = new List<Vector3>();
        //triangles = new List<int>();
        RenderNode(octree.GetRoot());

        //this.transform.position = new Vector3(0, 0, 0);        
        //UpdateMesh();
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

    // Update is called once per frame
    void Update()
    {
        //RenderNode(octree.GetRoot());
        //UpdateMesh();
    }
    private void Awake()
    {
        //mesh = GetComponent<MeshFilter>().mesh;
    }
    void OnDrawGizmos()
    {      
         //DrawNode(octree.GetRoot());
    }

    private Color minColor = new Color(1, 1, 1, 1f);
    private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

    private void DrawNode(Octree<int>.OctreeNode<int> node, int nodeDepth = 0)
    {
        if (!node.IsLeaf())
        {
            foreach (var subnode in node.Nodes)
            {
                DrawNode(subnode, nodeDepth + 1);
            }
            Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
        }
        else
            Gizmos.color = Color.green;       
        Gizmos.DrawWireCube(node.Position, Vector3.one * node.Size);
    }

    private void RenderNode(Octree<int>.OctreeNode<int> node, int nodeDepth = 0)
    {        
        if (!node.IsLeaf())
        {
            foreach (var subnode in node.Nodes)
            {
                RenderNode(subnode, nodeDepth + 1);
            }
        }

        if (PaintOnlyEmpty == true)
        {
            if (node.IsEmptyNode == false)
            {
                //Debug.Log(node.bottomX.ToString() + " " + node.upperX.ToString() + " " +
                //    node.bottomY.ToString() + " " + node.upperY.ToString() + " " + node.bottomZ.ToString() + " " + node.upperZ.ToString()
                //    + " " + node.IsEmptyNode + node.IsLeaf());

                if (node.IsLeaf())
                {
                    gameObject.AddComponent<CreateCube>().UpdateMesh(node, new Vector3(node.Position.x, node.Position.z, node.Position.y));
                    //MakeCube(node.Size / 2, new Vector3(node.Position.x, node.Position.z, node.Position.y), node);
                    //UpdateMesh(node);
                }
            }           
        }
        else
        {
            if (node.IsLeaf())
            {
                gameObject.AddComponent<CreateCube>().UpdateMesh(node, new Vector3(node.Position.x, node.Position.z, node.Position.y));
                //MakeCube(node.Size / 2 * scale, node.Position, node);
                //UpdateMesh(node);
            }
        }
        //else
        //{
            //Debug.Log(node.bottomX.ToString() + " " + node.upperX.ToString() + " " +
               // node.bottomY.ToString() + " " + node.upperY.ToString() + " " + node.bottomZ.ToString() + " " + node.upperZ.ToString()
               // + " " + node.IsEmptyNode);
        //}
    }
    private void MakeCube(float cubeScale, Vector3 cubePos, Octree<int>.OctreeNode<int> node)
    {
        for (int i = 0; i < 6; i++)
        {
            MakeFace((Direction)i, cubeScale, cubePos, node);
        }
    }
    private void MakeFace(Direction dir, float faceScale, Vector3 facePos, Octree<int>.OctreeNode<int> node)
    {
        node.vertices.AddRange(CubeMeshDataRight.faceVertices(dir, faceScale, facePos));
        int vCount = node.vertices.Count;

        node.triangles.Add(vCount - 4);
        node.triangles.Add(vCount - 4 + 1);
        node.triangles.Add(vCount - 4 + 2);
        node.triangles.Add(vCount - 4);
        node.triangles.Add(vCount - 4 + 2);
        node.triangles.Add(vCount - 4 + 3);
    }
    public void UpdateMesh(Octree<int>.OctreeNode<int> node)
    {
        GameObject cube = new GameObject();
        //cube.isStatic = true;
        cube.tag = "Terrain";
        cube.layer = LayerMask.NameToLayer("whatIsTerrain");
        cube.transform.SetParent(GameObject.Find("Octree").transform);
        MeshFilter filter = cube.AddComponent<MeshFilter>();        

        node.mesh = new Mesh();

        //node.mesh.Clear();

        node.mesh.vertices = node.vertices.ToArray();
        node.mesh.triangles = node.triangles.ToArray();
        node.mesh.RecalculateNormals();

        filter.mesh = node.mesh;

        cube.AddComponent<MeshRenderer>();
        cube.GetComponent<Renderer>().material =  material;

        cube.AddComponent<MeshCollider>();
        cube.GetComponent<MeshCollider>().sharedMesh = node.mesh;

        cube.AddComponent<ShootingTerrain>();
        cube.GetComponent<ShootingTerrain>().node = node;
        //MeshCollider meshCol = GetComponent<MeshCollider>();
        //meshCol.sharedMesh = mesh;
    }
}
