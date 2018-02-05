using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLocal : MonoBehaviour {

    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };

    float moveVertical;
    float turning;
    float shootHorizontal;
    float shootVertical;
    string fire;

    public float m_RotSpeed;           // tank rotation speed modifier
    public float m_Speed;              // tank movement speed modifier
    public float m_TurretRotate;
    public GameObject m_Barrel;        // location for spawning shot projectiles

    public bool ded = false;


    private Rigidbody m_Vehicle;       // reference to the tank's physics component
    public GameObject m_Turret;       // reference to the child turret object
    public Material m_Outline;
    private float m_ReloadTimer;       // counts down to 0, tank can only shoot when not counting down

    public string m_PlayerName;

    // Use this for initialization
    void Start ()
    {
        m_Vehicle = GetComponent<Rigidbody>();
        m_Outline.SetColor("_OutlineColor",Colors[PlayerPrefs.GetInt("P1Colour")]);
        Debug.Log(Colors[PlayerPrefs.GetInt("P1Colour")].ToString());
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        Move();
        Shoot();

        if (ded)
        {
            transform.position = FindFurthestTarget("Respawn").transform.position;            
            ded = false;
        }
        
    }

    private void Move()
    {
        m_PlayerName = transform.parent.name;

        switch (m_PlayerName)
        {
            case "Player1":
                moveVertical =    transform.parent.GetComponent<PlayerOneControl>().moveVertical;
                turning =         transform.parent.GetComponent<PlayerOneControl>().turning;
                shootHorizontal = transform.parent.GetComponent<PlayerOneControl>().shootHorizontal;
                shootVertical =   transform.parent.GetComponent<PlayerOneControl>().shootVertical;
                fire =            transform.parent.GetComponent<PlayerOneControl>().fire;
                break;
            case "Player2":
                moveVertical = transform.parent.GetComponent<PlayerTwoControl>().moveVertical;
                turning = transform.parent.GetComponent<PlayerTwoControl>().turning;
                shootHorizontal = transform.parent.GetComponent<PlayerTwoControl>().shootHorizontal;
                shootVertical = transform.parent.GetComponent<PlayerTwoControl>().shootVertical;
                fire = transform.parent.GetComponent<PlayerTwoControl>().fire;
                break;
        }

        


        Vector3 turretRotate = new Vector3(0f, shootHorizontal * m_TurretRotate, 0f);

        Vector3 move = new Vector3(0f, 0f, moveVertical);
        Vector3 turnRot = new Vector3(0f, turning * m_RotSpeed * Time.deltaTime, 0f);

        m_Turret.transform.Rotate(turretRotate);       
        transform.Rotate(turnRot);

        m_Vehicle.AddRelativeForce(move * m_Speed);
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
        if (Input.GetButton(fire))
        {
            GameObject bullet = TankPooler.SharedInstance.GetPooledObject();  // get bullet object from pool
            if (bullet != null) // check if object pool returned a bullet
            {
                if (m_ReloadTimer <= 0.0f)    // only shoot if not waiting for reload
                {
                    bullet.transform.position = m_Barrel.transform.position;
                    bullet.transform.rotation = m_Barrel.transform.rotation;
                    bullet.SetActive(true);
                    m_ReloadTimer = bullet.GetComponent<ShellTravel>().m_ReloadSpeed;    // get reload time from projectile
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

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            Debug.Log("hit");
            m_Vehicle.transform.position = new Vector3(m_Vehicle.transform.position.x, 2f, m_Vehicle.transform.position.z);
        }
    }
}
