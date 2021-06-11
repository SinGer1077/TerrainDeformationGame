using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTerrain : MonoBehaviour
{
    public Octree<int>.OctreeNode<int> node;
    public bool isDeleted;
    // Start is called before the first frame update
   
    public void TakeDamage(Vector3 position, float explosionRange, LayerMask whatIsTerrain, int explosionDamage) //List<Collider> blocksPrevious
    {
        if (node.Size > 1)
        {
            Octree<int>.OctreeNode<int> nodeCopy = node;                 
            nodeCopy.Subdivide();            

            foreach (var subnode in nodeCopy.Nodes)
            {
                gameObject.AddComponent<CreateCube>().UpdateMesh(subnode, new Vector3(subnode.Position.x, subnode.Position.z, subnode.Position.y));
            }
            //blocksPrevious.Remove(this.gameObject.GetComponent<Collider>());
            this.isDeleted = true;
            Destroy(this.gameObject);
            Collider[] blocks = Physics.OverlapSphere(position, explosionRange, whatIsTerrain);
            //Debug.Log("После деления куба размером "+ node.Size + " попали по блокам " + blocks.Length);
            //if (blocks.Length < 50)
            //{
            //    List<Collider> listBlocks = new List<Collider>();
            //    for (int i = 0; i < blocks.Length; i++)
            //    {
            //        if (!blocksPrevious.Contains(blocks[i]))
            //        {
            //            listBlocks.Add(blocks[i]);
            //        }
            //    }
            //    Debug.Log(listBlocks.Count);

                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].gameObject.GetComponent<ShootingTerrain>().isDeleted == false)
                        blocks[i].GetComponent<ShootingTerrain>().TakeDamage(position, explosionRange, whatIsTerrain, explosionDamage);//, blocksPrevious
                }
            //}

        }
        else
        {
            //blocksPrevious.Remove(this.gameObject.GetComponent<Collider>());
            this.gameObject.GetComponent<ShootingTerrain>().isDeleted = true;
            Destroy(this.gameObject);
        }
    }
}
