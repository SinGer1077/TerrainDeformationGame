using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueRocketBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsTerrain;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;
    bool alreadyExpload = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //if (collisions > maxCollisions) 
        //    Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
            Explode();
    }

    private void Explode()
    {
        if (alreadyExpload == false)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                alreadyExpload = true;
            }

            Collider[] blocks = Physics.OverlapSphere(transform.position, explosionRange, whatIsTerrain);
            Debug.Log("При первом выстреле получили блоков " + blocks.Length);
            if (blocks.Length < 50)
            {
                List<Collider> listBlocks = new List<Collider>(blocks.Length);
                listBlocks.AddRange(blocks);
                for (int i = 0; i < blocks.Length; i++)
                {

                    blocks[i].GetComponent<ShootingTerrain>().TakeDamage(transform.position, explosionRange, whatIsTerrain, explosionDamage);//, listBlock
                }
            }
            // Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
            Invoke("Delay", 0.05f);
        }
    }

    private void Delay()
    {        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        if (collision.collider.CompareTag("Terrain") && explodeOnTouch)
            Explode();
    }

    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }
}
