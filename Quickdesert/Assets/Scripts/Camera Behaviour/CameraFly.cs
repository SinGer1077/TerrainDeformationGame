using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFly : MonoBehaviour
{
    public float speed = 5f;
    Vector3 velocity = new Vector3(0, 0, 0);
    float rotate = 0;

    Collider coll;

    private void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamera();        
    }
    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotate++;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotate--;
        }

        this.transform.position = velocity;
        this.transform.rotation = Quaternion.Euler(0, rotate, 0);
    }
    
}
