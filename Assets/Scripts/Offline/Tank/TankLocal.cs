﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TankLocal : MonoBehaviour {

    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };

    float moveVertical;
    float turning;
    float shootHorizontal;
    float shootVertical;
    string fire;
    int lives;

    public float m_RotSpeed;           // tank rotation speed modifier
    public float m_Speed;              // tank movement speed modifier
    public float m_TurretRotate;
    public GameObject m_Barrel;        // location for spawning shot projectiles

    public List<Transform> HoverPoints = new List<Transform>();
    public float HoverHeight = 7;
    public float HoverForceFront = 200;
    public float HoverForceBack = 400;
    public bool isGrounded = false;

    public bool ded = false;

    GameObject Player;
    GameController control;

    private Rigidbody m_Vehicle;       // reference to the tank's physics component
    public GameObject m_Turret;       // reference to the child turret object
    public Material m_Outline;
    private float m_ReloadTimer;       // counts down to 0, tank can only shoot when not counting down

    public string m_PlayerName;

    // Use this for initialization
    void Start ()
    {
        control = GameObject.Find("GameController").GetComponent<GameController>();
        m_Vehicle = GetComponent<Rigidbody>();
        //m_Outline.SetColor("_OutlineColor",Colors[PlayerPrefs.GetInt("P1Colour")]);
        //Debug.Log(Colors[PlayerPrefs.GetInt("P1Colour")].ToString());
        m_Outline = GetComponent<Renderer>().material;
        m_PlayerName = transform.parent.name;
        
        switch (m_PlayerName)
        {
            case "Player1":
                m_Outline.SetColor("_OutlineColor", Colors[PlayerPrefs.GetInt("P1Colour")]);
                Debug.Log(Colors[PlayerPrefs.GetInt("P1Colour")].ToString());
                break;
            case "Player2":
                m_Outline.SetColor("_OutlineColor", Colors[PlayerPrefs.GetInt("P2Colour")]);
                Debug.Log(Colors[PlayerPrefs.GetInt("P2Colour")].ToString());
                break;
            case "Player3":
                m_Outline.SetColor("_OutlineColor", Colors[PlayerPrefs.GetInt("P3Colour")]);
                Debug.Log(Colors[PlayerPrefs.GetInt("P3Colour")].ToString());
                break;
            case "Player4":
                m_Outline.SetColor("_OutlineColor", Colors[PlayerPrefs.GetInt("P4Colour")]);
                Debug.Log(Colors[PlayerPrefs.GetInt("P4Colour")].ToString());
                break;
        }
        lives = PlayerPrefs.GetInt("GameLives");

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (!isGrounded)
        {
            m_Speed = 0f;
        }
        else
        {
            m_Speed = 50f;
        }

        Hover();
        Move();
        Shoot();

        if(Input.GetButtonDown("Reset"))
        {
            transform.rotation = Quaternion.identity;
            Debug.Log("1");
        }

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
                Player = transform.parent.gameObject;
                moveVertical = Player.GetComponent<PlayerOneControl>().moveVertical;
                turning = Player.GetComponent<PlayerOneControl>().turning;
                shootHorizontal = Player.GetComponent<PlayerOneControl>().shootHorizontal;
                shootVertical = Player.GetComponent<PlayerOneControl>().shootVertical;
                fire = Player.GetComponent<PlayerOneControl>().fire;
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
            GameObject bullet = GetComponent<TankPooler>().GetPooledObject();  // get bullet object from pool
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

    private void Hover()
    {


        //Lift
        for (int i = 0; i < 4; i++)
        {
            RaycastHit Hit;
            if (i > 1)
            {
                if (Physics.Raycast(HoverPoints[i].position, HoverPoints[i].TransformDirection(Vector3.down), out Hit, HoverHeight))
                    m_Vehicle.AddForceAtPosition((Vector3.up * HoverForceBack * Time.deltaTime) * Mathf.Abs(1 - (Vector3.Distance(Hit.point, HoverPoints[i].position) / HoverHeight)), HoverPoints[i].position);
                if (Hit.point != Vector3.zero)
                    Debug.DrawLine(HoverPoints[i].position, Hit.point, Color.blue);
            }
            else
            {
                if (Physics.Raycast(HoverPoints[i].position, HoverPoints[i].TransformDirection(Vector3.down), out Hit, HoverHeight))
                    m_Vehicle.AddForceAtPosition((Vector3.up * HoverForceFront * Time.deltaTime) * Mathf.Abs(1 - (Vector3.Distance(Hit.point, HoverPoints[i].position) / HoverHeight)), HoverPoints[i].position);
                if (Hit.point != Vector3.zero)
                    Debug.DrawLine(HoverPoints[i].position, Hit.point, Color.red);
            }
            if (Hit.point != Vector3.zero)
                isGrounded = true;
            else
                isGrounded = false;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "DeathPlane")
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
            }

            if(lives == 0)
            {
                control.EndMenu.SetActive(true);
                Time.timeScale = 0f;
                if (PlayerPrefs.GetInt("P1Lives") == 0)
                {
                    control.endText.text = "Player 2 Wins!";
                }
                else
                {
                    control.endText.text = "Player 1 Wins!";
                }
            }

            control.UpdateText();
            m_Vehicle.transform.position = FindFurthestTarget("Respawn").transform.position;
            transform.rotation = Quaternion.identity;
        }
    }
}
