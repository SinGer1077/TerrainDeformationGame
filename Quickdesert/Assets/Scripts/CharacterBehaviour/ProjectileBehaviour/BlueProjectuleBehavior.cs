using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectuleBehavior : MonoBehaviour
{
    public float speed;
    public VoxelDataStorage voxelDataStorage;

    float timer = 3f;
    float time;

    // Start is called before the first frame update
    void Start()
    {                   
        GetComponent<Rigidbody>().velocity = Camera.main.transform.TransformDirection(this.transform.position.normalized) * -speed;
        Debug.Log(GetComponent<Rigidbody>().velocity);
        Debug.Log(Camera.main.transform.TransformDirection(Input.mousePosition));

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > timer)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {
            Debug.Log(collision.contacts[0].point);
            Vector3 clickedBlockCoordinates = GetClickedBlockCoordinates(collision.contacts[0].point);
            DeleteVoxel(clickedBlockCoordinates);
            voxelDataStorage.voxelData.GenerateVoxelMesh(1f, 0.5f);
            voxelDataStorage.UpdateMesh();
        }
        Destroy(gameObject);
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
