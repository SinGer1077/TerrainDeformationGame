using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTerrain : MonoBehaviour
{
    public Octree<int>.OctreeNode<int> node;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(Vector3 position, float explosionRange, LayerMask whatIsTerrain, int explosionDamage)
    {
        if (node.Size > 1)
        {
            Octree<int>.OctreeNode<int> nodeCopy = node;                 
            nodeCopy.Subdivide();            

            foreach (var subnode in nodeCopy.Nodes)
            {
                gameObject.AddComponent<CreateCube>().UpdateMesh(subnode, new Vector3(subnode.Position.x, subnode.Position.z, subnode.Position.y));
            }

            Destroy(this.gameObject);
            //Collider[] blocks = Physics.OverlapSphere(position, explosionRange, whatIsTerrain);
            //for (int i = 0; i < blocks.Length; i++)
            //{
            //    blocks[i].GetComponent<ShootingTerrain>().TakeDamage(transform.position, explosionRange, whatIsTerrain, explosionDamage);
            //}
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
