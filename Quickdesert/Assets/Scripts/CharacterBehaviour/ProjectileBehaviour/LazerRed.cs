using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerRed :IPRojectile
{
    VoxelDataStorage voxelDataStorage;
    Collider collider;

  
    public int BulletCount { get; set; }
    public GunType Type { get; set; }
   
    public LazerRed(VoxelDataStorage storage, Collider characterCollider)
    {
        voxelDataStorage = storage;
        BulletCount = 10;
        Type = GunType.Hitman;
        collider = characterCollider;        
    }
    public void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //player.CurrentWeapon.Shot();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.tag != "Player")
                {
                    BulletCount -= 1;                    
                    Vector3 clickedBlockCoordinates = GetClickedBlockCoordinates(hit.point);
                    DeleteVoxel(clickedBlockCoordinates);
                    voxelDataStorage.voxelData.GenerateVoxelMesh(1f, 0.5f);
                    voxelDataStorage.UpdateMesh();
                }
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

    public void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BulletCount = 10;
        }
    }
}
