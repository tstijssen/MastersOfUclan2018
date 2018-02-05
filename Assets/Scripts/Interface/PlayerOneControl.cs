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

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        moveVertical = Input.GetAxis("Accelerate");
        turning = Input.GetAxis("Horizontal");
        shootHorizontal = Input.GetAxis("RightStick X");
        shootVertical = Input.GetAxis("RightStick Y");

        dPadHor = Input.GetAxis("DpadHor");
        dPadVert = Input.GetAxis("DpadVert");
    }
}
