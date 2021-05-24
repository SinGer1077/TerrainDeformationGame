using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBlue : IPRojectile
{
    VoxelDataStorage voxelDataStorage;
    Collider collider;


    public int BulletCount { get; set; }
    public GunType Type { get; set; }
    public GameObject projectile { get; set; }
    public float speed { get; set; }


    public RocketBlue(VoxelDataStorage storage, Collider characterCollider)
    {
        voxelDataStorage = storage;
        BulletCount = 10;
        Type = GunType.Projectile;
        collider = characterCollider;        
    }    
    public void Shot()
    {

    }
    public void Reload()
    {

    }
}
