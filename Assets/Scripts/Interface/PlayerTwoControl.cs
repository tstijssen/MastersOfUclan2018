using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoControl : MonoBehaviour
{


    public float moveVertical;
    public float turning;
    public float shootHorizontal;
    public float shootVertical;
    public string fire = "Fire2";

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        moveVertical = Input.GetAxis("Accelerate2");
        turning = Input.GetAxis("Horizontal2");
        shootHorizontal = Input.GetAxis("RightStick X2");
        shootVertical = Input.GetAxis("RightStick Y2"); ;
    }
}
