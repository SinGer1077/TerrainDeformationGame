using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPRojectile
{
    int BulletCount { get; set; }    
    GunType Type { get; set; }
    void Shot();
    void Reload();
}
public enum GunType
{
    Hitman,
    Projectile
}