using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoxelDataStorage", menuName = "ScriptableObjects/VoxelDataStorage", order = 1)]
public class VoxelDataStorage : ScriptableObject
{
    public VoxelDataRight voxelData;
    public Mesh mesh;
    
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = voxelData.vertices.ToArray();
        mesh.triangles = voxelData.triangles.ToArray();
        mesh.RecalculateNormals();
    }

}
