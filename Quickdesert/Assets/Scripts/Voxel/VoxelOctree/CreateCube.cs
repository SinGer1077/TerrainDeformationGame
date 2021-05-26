using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{
    public Material material;
    public float scale = 1f;
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
    public void UpdateMesh(Octree<int>.OctreeNode<int> node, Vector3 position)
    {
        MakeCube(node.Size / 2 * scale, position, node);

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
        cube.GetComponent<Renderer>().material = material;

        cube.AddComponent<MeshCollider>();
        cube.GetComponent<MeshCollider>().sharedMesh = node.mesh;

        cube.AddComponent<ShootingTerrain>();
        cube.GetComponent<ShootingTerrain>().node = node;
        //MeshCollider meshCol = GetComponent<MeshCollider>();
        //meshCol.sharedMesh = mesh;
    }

}
