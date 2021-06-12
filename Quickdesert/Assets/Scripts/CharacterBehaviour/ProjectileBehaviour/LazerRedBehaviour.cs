using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LazerRedBehaviour : MonoBehaviour
{
    //Gun stats
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    public int explosionDamage;
    public float explosionRange;

    public LayerMask whatIsEnemies;
    public LayerMask whatIsTerrain;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI reloadingDisplay;
    public Slider reloadingProgress;

    public GameObject explosion;    
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    Player player;
    //bug fixing
    public bool allowInvoke = true;

    private void Awake()
    {

        bulletsLeft = magazineSize;
        readyToShoot = true;

        reloadingDisplay.SetText("");
        reloadingProgress.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player = GetComponent<PlayerGameplay>().player;

        if (player.CurrentWeapon is LazerRed)
        {
            MyInput();
            //Set ammo display, if it exist
            if (ammunitionDisplay != null)
                ammunitionDisplay.SetText(bulletsLeft / bulletPerTap + " / " + magazineSize / bulletPerTap);
        }
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets to 0
            bulletsShot = 0;

            Shot();
        }
    }
    private void Shot()
    {
        readyToShoot = false;
        if (fpsCam != null)
        {
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.tag != "Wall")
                {
                    if (hit.transform.gameObject.tag != "Player")
                    {

                    }
                    Explode(hit.point);
                }
            }
        }
        else
        {
            Vector3 fromPosition = attackPoint.transform.position;
            Vector3 toPosition = new Vector3(19, 9, 19);
            Vector3 direction = toPosition - fromPosition;
            RaycastHit hit;
            if (Physics.Raycast(fromPosition, direction, out hit, 100))
            {
                if (hit.transform.gameObject.tag != "Wall")
                {
                    if (hit.transform.gameObject.tag != "Player")
                    {

                    }
                    Explode(hit.point);
                }
            }
        }
        

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
    private void Explode(Vector3 hitpoint)
    {
        //if (explosion != null)
        //    Instantiate(explosion, hitpoint, Quaternion.identity);

        // Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);

        if (explosion != null)
        {
            Instantiate(explosion, hitpoint, Quaternion.identity);            
        }
        Collider[] blocks = Physics.OverlapSphere(hitpoint, explosionRange, whatIsTerrain);
        Debug.Log("При первом выстреле получили блоков " + blocks.Length);
        if (blocks.Length < 50)
        {
            List<Collider> listBlocks = new List<Collider>(blocks.Length);
            listBlocks.AddRange(blocks);
            for (int i = 0; i < blocks.Length; i++)
            {

                blocks[i].GetComponent<ShootingTerrain>().TakeDamage(hitpoint, explosionRange, whatIsTerrain, explosionDamage);//, listBlock
            }
        }
        Collider[] enemies = Physics.OverlapSphere(hitpoint, explosionRange, whatIsEnemies); 
        for (int i=0; i<enemies.Length; i++)
        {
            if (enemies[i].tag == "BotComponent")
            {
                enemies[i].gameObject.GetComponentInParent<BotGameplay>().TakeDamage(explosionDamage);
            }
            //enemies[i].gameObject.GetComponent<BotGameplay>().TakeDamage(explosionDamage);
        }

    }    

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        float reloadingTime = 0;
        if (reloadingDisplay != null)
            reloadingDisplay.SetText("Reloading...");
        if (reloadingProgress != null)
        {
            reloadingProgress.gameObject.SetActive(true);
            reloadTime += Time.deltaTime;
            reloadingProgress.value = reloadingTime / reloadTime;
        }

        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        reloadingProgress.gameObject.SetActive(false);
        reloadingDisplay.SetText("");
    }
}
