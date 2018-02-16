using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneControl : MonoBehaviour
{
    public float moveVertical;
    public float turning;
    public float shootHorizontal;
    public float shootVertical;

    public float dPadVert;
    public float dPadHor;

    public string fire = "Fire1";
    public string brake = "Brake1";



    public int vehicle = 0;

    // Use this for initialization
    void Start ()
    {
        PlayerPrefs.SetInt("P1Lives", 5);    
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // joystick controls
        if (Input.GetJoystickNames().Length > 0)
        {
            moveVertical = Input.GetAxis("Accelerate");
            turning = Input.GetAxis("Horizontal");
            shootHorizontal = Input.GetAxis("RightStick X");
            shootVertical = Input.GetAxis("RightStick Y");

            dPadHor = Input.GetAxis("DpadHor");
            dPadVert = Input.GetAxis("DpadVert");
        }
        // keyboard controls
        else
        {
            moveVertical = Input.GetAxis("VerticalKeyB");
            turning = Input.GetAxis("HorizontalKeyB");
            shootHorizontal = Input.GetAxis("TurretHorizontalKeyB");
            shootVertical = Input.GetAxis("TurretVerticalKeyB");

            dPadHor = Input.GetAxis("TurretHorizontalKeyB");
            dPadVert = Input.GetAxis("TurretVerticalKeyB");
            fire = "FireKeyB";
        }

    }
}
