using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeData
{
    OctreeData[,,] childOctrees = new OctreeData[2, 2, 2];
    OctreeData Parent;
    int[] directionsEdge;

    public OctreeData(int north, int south, int east, int west, int down, int up, OctreeData parent)
    {
        directionsEdge = new int[6]
        {
            north, south, east, west, up, down
        };
        Parent = parent;

        int childSouth = (north + south) / 2;
        int childWest = (east + west) / 2;
        int childUp = (up + down) / 2;

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {                    
                    if (childSouth - north >= 1 && childWest - east >= 1 && childUp - down >= 1 && parent!=this)
                    {
                        Debug.Log((north + childSouth * x).ToString() + " " + (north + childSouth * (x + 1)).ToString() + " " +
                            (east + childWest * y, east + childWest * (y + 1)).ToString() + " " +
                            (down + childUp * z, down + childUp * (z + 1)).ToString());
                        childOctrees[x, y, z] = new OctreeData(north + childSouth * x, north + childSouth * (x + 1),
                            east + childWest * y, east + childWest * (y + 1),
                            down + childUp * z, down + childUp * (z + 1), this);                        
                    }
                }
            }
        }
    }    
}

public enum OctreeIndex
{
    BottomLeftFront = 0, //000,
    BottomRightFront = 2, //010,
    BottomRightBack = 3, //011,
    BottomLeftBack = 1, //001,
    TopLeftFront = 4, //100,
    TopRightFront = 6, //110,
    TopRightBack = 7, //111,
    TopLeftBack = 5, //101,
}

public class Octree<TType>
{
    private OctreeNode<TType> node;
    private int depth;
    

    public Octree(Vector3 position, float size, int depth, VoxelDataRight data)
    {
        node = new OctreeNode<TType>(position, size, data);
        node.Subdivide(depth);
        
    }

    public class OctreeNode<TType>
    {
        Vector3 position;
        float size;
        OctreeNode<TType>[] subNodes;
        IList<TType> value;
        VoxelDataRight voxelGrid;
        bool isEmpty;
        bool subNodeIsFull;

        public List<Vector3> vertices;
        public List<int> triangles;
        public Mesh mesh;

        public float bottomX;
        public float upperX;
        public float bottomY;
        public float upperY;
        public float bottomZ;
        public float upperZ;


        public OctreeNode(Vector3 pos, float size, VoxelDataRight data)
        {
            position = pos;
            this.size = size;
            voxelGrid = data;
            vertices = new List<Vector3>();
            triangles = new List<int>();
            //Debug.Log(position.ToString() + " " + this.size);

        }

        public IEnumerable<OctreeNode<TType>> Nodes
        {
            get { return subNodes; }
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public float Size
        {
            get { return size; }
        }
        public bool IsEmptyNode
        {
            get { return isEmpty; }
            set { isEmpty = value; }
        }
        public bool IsFullNode
        {
            get { return subNodeIsFull; }
            set { subNodeIsFull = value; }
        }

        public void Subdivide(int depth = 0)
        {
            subNodes = new OctreeNode<TType>[8];
            for (int i = 0; i < subNodes.Length; ++i)
            {
                Vector3 newPos = position;
                if ((i & 4) == 4)
                {
                    newPos.y += size * 0.25f;
                }
                else
                {
                    newPos.y -= size * 0.25f;
                }

                if ((i & 2) == 2)
                {
                    newPos.x += size * 0.25f;
                }
                else
                {
                    newPos.x -= size * 0.25f;
                }

                if ((i & 1) == 1)
                {
                    newPos.z += size * 0.25f;
                }
                else
                {
                    newPos.z -= size * 0.25f;
                }

                OctreeNode<TType> newNode = new OctreeNode<TType>(newPos, size * 0.5f, voxelGrid);
                newNode.IsFullNode = SubNodeIsFull(newNode);
                subNodes[i] = newNode;
                if (depth > 0 && !subNodes[i].IsFullNode) //&& !SubNodeIsFull(subNodes[i])
                {
                    subNodes[i].Subdivide(depth - 1);
                }
            }
        }
        private bool SubNodeIsFull(OctreeNode<TType> subNode)
        {
            subNode.bottomX = subNode.position.x - subNode.Size * 0.5f;
            subNode.upperX = subNode.position.x + subNode.Size * 0.5f;
            subNode.bottomY = subNode.position.y - subNode.Size * 0.5f;
            subNode.upperY = subNode.position.y + subNode.Size * 0.5f;
            subNode.bottomZ = subNode.position.z - subNode.Size * 0.5f;
            subNode.upperZ = subNode.position.z + subNode.Size * 0.5f;

            //Debug.Log(subNode.Size);
            //Debug.Log(bottomX.ToString() + " " + upperX.ToString() + " " +
            //    bottomY.ToString() + " " + upperY.ToString() + " " + bottomZ.ToString() + " " + upperZ.ToString());

            bool subNodeIsFull;
            int emptyBlocks = 0;
            int voxels = 0;           

            for (int x = (int)subNode.bottomX; x < subNode.upperX; x++)
            {
                for (int y = (int)subNode.bottomY; y < subNode.upperY; y++)
                {
                    for (int z = (int)subNode.bottomZ; z < subNode.upperZ; z++)
                    {
                        if (voxelGrid.data.GetLength(0) <= x || voxelGrid.data.GetLength(1) <= y
                            || voxelGrid.data.GetLength(2) <= z)
                        {
                            emptyBlocks++;
                        }
                        else {
                            if (voxelGrid.data[x, y, z] == 0)
                                emptyBlocks++;
                            else
                                voxels++;
                        }
                    }
                }
            }
            if (emptyBlocks != 0 && voxels != 0)
            {
                subNodeIsFull = false;
                subNode.IsEmptyNode = false;
            }
            else
            {
                subNodeIsFull = true;
                if (emptyBlocks != 0 && voxels == 0)//if (emptyBlocks == (subNode.upperX - subNode.bottomX) * (subNode.upperY - subNode.bottomY) * (subNode.upperZ - -subNode.bottomZ))//if (emptyBlocks != 0 && voxels == 0)
                    subNode.IsEmptyNode = true;
                else
                    subNode.IsEmptyNode = false;
            }
            return subNodeIsFull;
        }

        public bool IsLeaf()
        {
            return subNodes == null;
        }
        private void ChangeGrid(float bottomX, float upperX, float bottomY, float upperY, float bottomZ, float upperZ)
        {
            voxelGrid.data[(int)bottomX, (int)bottomY, (int)bottomZ] = (int)(upperX - bottomX);
            for (int x = (int)bottomX; x < upperX; x++)
            {
                for (int y = (int)bottomY; y < upperY; y++)
                {
                    for (int z = (int)bottomZ; z < upperZ; z++)
                    {
                        
                    }
                }
            }
        }
    }

    

    private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.y > nodePosition.y ? 4 : 0;
        index |= lookupPosition.x > nodePosition.x ? 2 : 0;
        index |= lookupPosition.z > nodePosition.z ? 1 : 0;

        return index;
    }

    public OctreeNode<TType> GetRoot()
    {
        return node;
    }    
}