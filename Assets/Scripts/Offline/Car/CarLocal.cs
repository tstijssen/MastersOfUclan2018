using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLocal : MonoBehaviour {

    public float m_Speed;
    public float m_TurnSpeed;


    float moveVertical;
    float turning;
    float shootHorizontal;
    float shootVertical;
    string fire;

    public GameObject m_Barrel1;        // location for spawning shot projectiles
    public GameObject m_Barrel2;        // location for spawning shot projectiles

    public Vector3 m_Speedometer;
    public bool ded = false;
    public bool altguns = false;
    public bool fired = false;
    public bool m_IsGrounded = true;

    private float m_RotationLimit = 30f;

    private Rigidbody m_Vehicle;       // reference to the Car's physics component

    private float m_ReloadTimer;       // counts down to 0, tank can only shoot when not counting down

    public string m_PlayerName;

    private Vector3 m_VehicleStartPos;
    private Quaternion m_VehicleStartRot;

    // Use this for initialization
    void Start ()
    {
        m_Vehicle = GetComponent<Rigidbody>();
        m_VehicleStartPos = m_Vehicle.transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        m_PlayerName = transform.parent.name;

        switch (m_PlayerName)
        {
            case "Player1":
                moveVertical = transform.parent.GetComponent<PlayerOneControl>().moveVertical;
                turning = transform.parent.GetComponent<PlayerOneControl>().turning;
                shootHorizontal = transform.parent.GetComponent<PlayerOneControl>().shootHorizontal;
                shootVertical = transform.parent.GetComponent<PlayerOneControl>().shootVertical;
                fire = transform.parent.GetComponent<PlayerOneControl>().fire;
                break;
            case "Player2":
                moveVertical = transform.parent.GetComponent<PlayerTwoControl>().moveVertical;
                turning = transform.parent.GetComponent<PlayerTwoControl>().turning;
                shootHorizontal = transform.parent.GetComponent<PlayerTwoControl>().shootHorizontal;
                shootVertical = transform.parent.GetComponent<PlayerTwoControl>().shootVertical;
                fire = transform.parent.GetComponent<PlayerTwoControl>().fire;
                break;
        }

        Vector3 move = new Vector3(0f, 0f, moveVertical) ;
        Vector3 turnRot = new Vector3(0f, turning * m_TurnSpeed * Time.deltaTime, 0f) ;

        if (m_Vehicle.velocity.magnitude > 0f)
        {
            transform.Rotate(turnRot * m_Vehicle.velocity.magnitude * Time.deltaTime);
        }
        if (m_Vehicle.velocity.magnitude < 0f)
        {
            transform.Rotate(turnRot * -m_Vehicle.velocity.magnitude * Time.deltaTime);
        }

        if (m_IsGrounded)
        {
            m_Vehicle.AddRelativeForce(move * m_Speed);
        }

        Shoot();


        if (ded)
        {
            transform.position = FindFurthestTarget("Respawn").transform.position;
            ded = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = m_VehicleStartPos;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

    }

    private void Shoot()
    {
        
        //Reloading
        if (m_ReloadTimer > 0.0f) 
        {
            m_ReloadTimer -= Time.deltaTime;
        }
        else
        {
            m_ReloadTimer = 0.0f;
        }

        //Shooting
        if (Input.GetButton(fire) && !fired)
        {
            GameObject bullet = CarPooler.SharedInstance.GetPooledObject();  // get bullet object from pool
            if (bullet != null) // check if object pool returned a bullet
            {
                if (m_ReloadTimer <= 0.0f)    // only shoot if not waiting for reload
                {
                    if (altguns)
                    {
                        bullet.transform.position = m_Barrel1.transform.position;
                        bullet.transform.rotation = m_Barrel1.transform.rotation;
                        altguns = !altguns;
                    }
                    else
                    {
                        bullet.transform.position = m_Barrel2.transform.position;
                        bullet.transform.rotation = m_Barrel2.transform.rotation;
                        altguns = !altguns;
                    }
                    
                    
                    bullet.SetActive(true);
                    m_ReloadTimer = bullet.GetComponent<BulletTravel>().m_ReloadSpeed;    // get reload time from projectile
                }
            }
        }
    }

    public GameObject FindFurthestTarget(string trgt)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(trgt);

        GameObject furthest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position + position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                furthest = go;
                distance = curDistance;
            }
        }

        return furthest;
    }


    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag == "Player 1")
        {
            ded = true;

        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_IsGrounded = false;
        }
    }
}
