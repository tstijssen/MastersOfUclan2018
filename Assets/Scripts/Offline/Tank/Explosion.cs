using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float explosionRadius;
    public float explosionPower;

    private GameObject enemy;
    private CarLocal enemyscript;
    ParticleSystem Splode;

    // Use this for initialization
    void Start ()
    {
        //enemy = GameObject.Find("Car");
        //enemyscript = enemy.GetComponent<CarLocal>();
        //Splode = GetComponent<ParticleSystem>();
        //var splodeCollision = Splode.collision;
        //splodeCollision.SetPlane(0, enemy.transform);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 5.0f);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    //void OnParticleCollision(GameObject other)
    //{
        
    //    if (other.gameObject.tag == "Player 2")
    //    {            
    //        enemyscript.ded = true;
    //    }
    //}
}
