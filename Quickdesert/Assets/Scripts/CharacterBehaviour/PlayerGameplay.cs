using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using TMPro;

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
    public GameObject playerBody;
    public Material standartMaterial;

    public GameObject pauseMenu;
    public GameObject controlMenu;
    public GameObject finishMenu;
    public Text finishMessage;
    public Camera fpsCamera;

    public TextMeshProUGUI hpDisplay;

    GameObject[] bots;

    // Start is called before the first frame update
    void Start()
    {
        //terrainCollider = GameObject.Find("VoxelTerrain").GetComponent<Collider>();
       // terrainMesh = GameObject.Find("VoxelTerrain").GetComponent<MeshCollider>();
       // terrainMesh.sharedMesh = voxelDataStorage.mesh;

        player = new Player();
        LazerRed weapon = new LazerRed(voxelDataStorage, terrainCollider);        
        
        WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/LazerRed", typeof(Material)) as Material;

        player.CurrentWeapon = weapon;
        ResumeGame();
        if (hpDisplay != null)
            hpDisplay.SetText("HP: " + player.HP + "/" + "100");

        bots = GameObject.FindGameObjectsWithTag("Bot");
        foreach (GameObject bot in bots)
            bot.SetActive(false);
        //Debug.Log(player.CurrentWeapon);        
    }

    // Update is called once per frame
    void Update()
    {
        // terrainMesh = GameObject.Find("VoxelTerrain").GetComponent<MeshCollider>();
        // terrainMesh.sharedMesh = voxelDataStorage.mesh;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf == false)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
            if (controlMenu.activeSelf == true)
            {
                controlMenu.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {            
            if (bots[0].activeSelf == true) 
            {
                foreach (GameObject bot in bots)
                    bot.SetActive(false);
            }
            else
            {
                foreach (GameObject bot in bots)
                    bot.SetActive(true);
            }
        }
        int flag = 0;
        foreach (GameObject bot in bots)
        {
            if (bot == null)
                flag += 1;
        }
        if (flag == 5)
        {
            PauseGame();
            finishMenu.SetActive(true);
            finishMessage.text = "You won!!!";
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GetComponent<LazerRedBehaviour>().enabled = false;
        GetComponent<BlueRocketBehaviour>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GetComponent<LazerRedBehaviour>().enabled = true;
        GetComponent<BlueRocketBehaviour>().enabled = true;
        GetComponent<FirstPersonController>().enabled = true;
        Cursor.visible = false;
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
    public void TakeDamage(int damage)
    {
        player.HP -= damage;
        playerBody.GetComponent<Renderer>().material = Resources.Load("Materials/Hitted", typeof(Material)) as Material;
        if (hpDisplay != null)
            hpDisplay.SetText("HP: " + player.HP + "/" + "100");
        if (player.HP <= 0)
        {
            PauseGame();
            finishMenu.SetActive(true);
            finishMessage.text = "You died(";
        }
        Invoke("InvokeHit", 0.5f);
    }
    public void InvokeHit()
    {
        playerBody.GetComponent<Renderer>().material = standartMaterial;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.name == "WallDown")
        //{
        //    PauseGame();
        //    finishMenu.SetActive(true);
        //    finishMessage.text = "You died(";
        //}
    }
}
