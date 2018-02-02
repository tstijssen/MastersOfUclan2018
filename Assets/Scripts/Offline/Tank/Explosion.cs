using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    private GameObject enemy;
    private CarLocal enemyscript;
    ParticleSystem Splode;

    // Use this for initialization
    void Start ()
    {
        enemy = GameObject.Find("Car");
        enemyscript = enemy.GetComponent<CarLocal>();
        Splode = GetComponent<ParticleSystem>();
        var splodeCollision = Splode.collision;
        splodeCollision.SetPlane(0, enemy.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        
        if (other.gameObject.tag == "Player 2")
        {            
            enemyscript.ded = true;
        }
    }
}
