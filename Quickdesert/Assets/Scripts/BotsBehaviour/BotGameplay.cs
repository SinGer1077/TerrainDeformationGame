using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGameplay : MonoBehaviour
{
    public Player player;
    public VoxelDataStorage voxelDataStorage;
    Collider terrainCollider;

    public GameObject projectile;
    public GameObject gun;
    public GameObject WeaponModel;


    //bots parameters
    //jump
    public float timeBetweenJump;
    bool readyToJump;
    public int jumpForce;

    //move
    public float speed;
    public float timeBetweenChangeWay;
    bool readyToChange;
    int[] dx = { 1, 0, -1, 0 };
    int[] dy = { 0, 1, 0, -1 };

    private void Awake()
    {
        readyToJump = true;
        readyToChange = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = new Player();

        LazerRed weapon = new LazerRed(voxelDataStorage, terrainCollider);
        WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/LazerRed", typeof(Material)) as Material;
        player.CurrentWeapon = weapon;
    }

    // Update is called once per frame
    void Update()
    {
        //бот двигается постоянно
        Movement();
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    player.CurrentWeapon = new LazerRed(voxelDataStorage, GetComponent<Collider>());
        //    Debug.Log("Red lazer");
        //    WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/LazerRed", typeof(Material)) as Material;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    player.CurrentWeapon = new RocketBlue(voxelDataStorage, GetComponent<Collider>());
        //    WeaponModel.GetComponent<Renderer>().material = Resources.Load("Materials/RocketBlue", typeof(Material)) as Material;
        //    Debug.Log("Rocket blue");
        //}
    }
    void Movement()
    {
        if (readyToJump)
        {
            Jump();
            Invoke("ResetJump", timeBetweenJump);
        }
        if (readyToChange)
        {
            readyToChange = false;
            int way_to_move = Random.Range(0, 4);
            this.transform.GetComponent<Rigidbody>().velocity = new Vector3(dx[way_to_move] * speed, 0, dy[way_to_move] * speed);
            Invoke("ResetMove", timeBetweenChangeWay);
        }
    }
    void Jump()
    {
        readyToJump = false;
        GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
    void ResetJump()
    {
        readyToJump = true;
    }
    void ResetMove()
    {
        readyToChange = true;
    }
    public void TakeDamage(int damage)
    {
        player.HP -= damage;
        if (player.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
