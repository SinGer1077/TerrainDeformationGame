using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplay : MonoBehaviour
{
    public Player player;
    public VoxelDataStorage voxelDataStorage;
    //Collider collider;

    Collider terrainCollider;
    MeshCollider terrainMesh;

    public GameObject projectile;
    public GameObject gun;
    public GameObject WeaponModel;
    // Start is called before the first frame update
    void Start()
    {
        terrainCollider = GameObject.Find("VoxelTerrain").GetComponent<Collider>();
        terrainMesh = GameObject.Find("VoxelTerrain").GetComponent<MeshCollider>();
        terrainMesh.sharedMesh = voxelDataStorage.mesh;

        player = new Player();
        LazerRed weapon = new LazerRed(voxelDataStorage, terrainCollider);        
        
        WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/LazerRed", typeof(Material)) as Material;

        player.CurrentWeapon = weapon;
        Debug.Log(player.CurrentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        terrainMesh = GameObject.Find("VoxelTerrain").GetComponent<MeshCollider>();
        terrainMesh.sharedMesh = voxelDataStorage.mesh;

        //if (player.CurrentWeapon.Type == GunType.Hitman)
        //{
        //    player.CurrentWeapon.Shot();
        //}
        //else
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {                
        //        Instantiate(projectile, gun.transform.position,
        //            Quaternion.identity);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.CurrentWeapon = new LazerRed(voxelDataStorage, GetComponent<Collider>());
            Debug.Log("Red lazer");
            WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/LazerRed", typeof(Material)) as Material;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.CurrentWeapon = new RocketBlue(voxelDataStorage, GetComponent<Collider>());
            WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/RocketBlue", typeof(Material)) as Material;
            Debug.Log("Rocket blue");
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
            if (Mathf.Floor((hitPoint.x % 1) * 10) >= 5 && x < voxelDataStorage.voxelData.data.GetLength(0) - 1)
                x++;
        }
        else
            if (Mathf.Floor((hitPoint.x % 1) * 10) > 5 && x < voxelDataStorage.voxelData.data.GetLength(0) - 1)
            x++;

        //correct y
        if (Camera.main.transform.position.y < y)
        {
            if (Mathf.Floor((hitPoint.y % 1) * 10) >= 5 && y < voxelDataStorage.voxelData.data.GetLength(2) - 1)
                y++;
        }
        else
             if (Mathf.Floor((hitPoint.y % 1) * 10) > 5 && y < voxelDataStorage.voxelData.data.GetLength(2) - 1)
            y++;

        //correct z
        if (Camera.main.transform.position.z < z)
        {
            if (Mathf.Floor((-hitPoint.z % 1) * 10) > 5 && -z < voxelDataStorage.voxelData.data.GetLength(1) - 1)
                z--;
        }
        else
            if (Mathf.Floor((-hitPoint.z % 1) * 10) >= 5 && -z < voxelDataStorage.voxelData.data.GetLength(1) - 1)
            z--; 
        Vector3 block = new Vector3(x, z, y);
        return block;
    }
    private void DeleteVoxel(Vector3 block)
    {
        if (voxelDataStorage.voxelData.GetCell((int)block.x, -(int)block.y, (int)block.z) != 0)
            voxelDataStorage.voxelData.data[(int)block.x, -(int)block.y, (int)block.z] = 0;
    }
}
