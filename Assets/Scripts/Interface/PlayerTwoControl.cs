using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoControl : MonoBehaviour
{


    public float moveVertical;
    public float turning;
    public float shootHorizontal;
    public float shootVertical;
    public float dPadVert;
    public float dPadHor;

    public string brake = "Brake2";
    public string fire = "Fire2";

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        moveVertical = Input.GetAxis("Accelerate2");
        turning = Input.GetAxis("Horizontal2");
        shootHorizontal = Input.GetAxis("RightStick X2");
        shootVertical = Input.GetAxis("RightStick Y2");
        dPadHor = Input.GetAxis("DpadHor2");
        dPadVert = Input.GetAxis("DpadVert2");
    }
}
