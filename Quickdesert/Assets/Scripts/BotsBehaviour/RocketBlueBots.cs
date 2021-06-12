using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RocketBlueBots : MonoBehaviour
{
    //bullet
    public GameObject bullet;
    public GameObject targetPlayer;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    //public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    

    Player player;
    //bug fixing
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;        
    }

    // Update is called once per frame
    void Update()
    {
        player = GetComponent<BotGameplay>().player;

        
            MyInput();
            //Set ammo display, if it exist          
        
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        shooting = true;

        //Reloading       
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets to 0
            bulletsShot = 0;
            Shot();
        }
        shooting = false;
        Invoke("InvokeShooting", 2f);
    }
    void InvokeShooting()
    {
        shooting = true;
    }
    private void Shot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        //Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Ray ray = new Ray(attackPoint.position, targetPlayer.transform.position);
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100);

        //Calculate direction from attackPoint to a targetPoint
        Vector3 directionWithoutSpeed = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpeed + new Vector3(x, y, 0);

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position,
            Quaternion.identity);

        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up.normalized * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
        {
            GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        float reloadingTime = 0;
     

        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
     
    }

}