using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankLocal : MonoBehaviour {

    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };

    float moveVertical;
    float turning;
    float shootHorizontal;
    float shootVertical;
    string fire;
    int lives = 5;

    public float m_StartingHealth = 100f;
    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.

    public float m_RotSpeed;           // tank rotation speed modifier
    public float m_Speed;              // tank movement speed modifier
    public float m_TurretRotate;
    public GameObject m_Barrel;        // location for spawning shot projectiles

    public bool ded = false;

    GameObject Player;
    GameController control;

    private Rigidbody m_Vehicle;       // reference to the tank's physics component
    public GameObject m_Turret;       // reference to the child turret object
    public Material m_Outline;
    private float m_ReloadTimer;       // counts down to 0, tank can only shoot when not counting down
    private float m_CurrentHealth;                      // How much health the tank currently has.
    public string m_PlayerName;

    // Use this for initialization
    void Start ()
    {
        control = GameObject.Find("GameController").GetComponent<GameController>();
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
                Player =          transform.parent.gameObject;
                moveVertical =    Player.GetComponent<PlayerOneControl>().moveVertical;
                turning =         Player.GetComponent<PlayerOneControl>().turning;
                shootHorizontal = Player.GetComponent<PlayerOneControl>().shootHorizontal;
                shootVertical =   Player.GetComponent<PlayerOneControl>().shootVertical;
                fire =            Player.GetComponent<PlayerOneControl>().fire;               
                break;
            case "Player2":
                moveVertical = transform.parent.GetComponent<PlayerTwoControl>().moveVertical;
                turning = transform.parent.GetComponent<PlayerTwoControl>().turning;
                shootHorizontal = transform.parent.GetComponent<PlayerTwoControl>().shootHorizontal;
                shootVertical = transform.parent.GetComponent<PlayerTwoControl>().shootVertical;
                fire = transform.parent.GetComponent<PlayerTwoControl>().fire;
                break;
        }

        Vector3 turretRotate = new Vector3(0f, shootHorizontal * m_TurretRotate * Time.deltaTime, 0f);

        Vector3 move = new Vector3(0f, 0f, moveVertical * m_Speed * Time.deltaTime);
        Vector3 turnRot = new Vector3(0f, turning * m_RotSpeed * Time.deltaTime, 0f);

        m_Turret.transform.Rotate(turretRotate);       
        transform.Rotate(turnRot);

        //Vector3 movement = transform.forward * moveVertical * m_Speed * Time.deltaTime;
        //m_Vehicle.MovePosition(m_Vehicle.position + movement);

        m_Vehicle.AddRelativeForce(move);
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
        if (Input.GetButton(fire) && m_ReloadTimer <= 0.0f)
        {
            GameObject bullet = TankPooler.SharedInstance.GetPooledObject();  // get bullet object from pool
            if (bullet != null) // check if object pool returned a bullet
            {
                m_ReloadTimer = bullet.GetComponent<ShellTravel>().m_ReloadSpeed;    // get reload time from projectile
                bullet.transform.position = m_Barrel.transform.position;
                bullet.transform.rotation = m_Barrel.transform.rotation;
                bullet.SetActive(true);
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
        //if(other.gameObject.tag == "Ground")
        //{
        //    Debug.Log("hit");
        //    m_Vehicle.transform.position = new Vector3(m_Vehicle.transform.position.x, 2f, m_Vehicle.transform.position.z);
        //}
    }

    public void TakeDamage()
    {
        m_CurrentHealth -= 20.0f;
        SetHealthUI();
        if (m_CurrentHealth <= 0f)
        {
            OnDeath();
            Debug.Log("HealthHit " + m_CurrentHealth.ToString());

        }
    }

    private void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = m_CurrentHealth;

        //// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        //m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {

        lives--;
        switch (m_PlayerName)
        {
            case "Player1":
                PlayerPrefs.SetInt("P1Lives", lives);
                break;
            case "Player2":
                PlayerPrefs.SetInt("P2Lives", lives);
                break;
            case "Player3":
                PlayerPrefs.SetInt("P2Lives", lives);
                break;
        }
        m_CurrentHealth = m_StartingHealth;
        SetHealthUI();
        control.UpdateText();
        Debug.Log("Kill");
        m_Vehicle.transform.position = new Vector3(0f, 0f, 0f); //Set Position to a respawn point
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathPlane")
        {
            OnDeath();
            Debug.Log("Deathplane");

        }
    }
}
