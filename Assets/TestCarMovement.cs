using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
public class TestCarMovement : MonoBehaviour {

    public float speed;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };

    float moveVertical;
    float turning;
    float shootHorizontal;
    float shootVertical;
    string fire;

    float brakes = 0;
    public string m_PlayerName;
    public Material m_Outline;

    //public Vector3 CenterOfMass;
    public Rigidbody rb;
    void Start()
    {
        Rigidbody m_Vehicle = GetComponent<Rigidbody>();
        m_Outline.SetColor("_OutlineColor", Colors[PlayerPrefs.GetInt("P1Colour")]);
        Debug.Log(Colors[PlayerPrefs.GetInt("P1Colour")].ToString());
    }
    public void FixedUpdate()
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
            //case "Player2":
            //    moveVertical = transform.parent.GetComponent<PlayerTwoControl>().moveVertical;
            //    turning = transform.parent.GetComponent<PlayerTwoControl>().turning;
            //    shootHorizontal = transform.parent.GetComponent<PlayerTwoControl>().shootHorizontal;
            //    shootVertical = transform.parent.GetComponent<PlayerTwoControl>().shootVertical;
            //    fire = transform.parent.GetComponent<PlayerTwoControl>().fire;
            //    break;
        }
        float motor = maxMotorTorque * moveVertical * speed;
        float steering = maxSteeringAngle * turning;

        //float motor = maxMotorTorque * Input.GetAxis("Vertical") * speed;
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");


        if (Input.GetButton("Brake1"))
        {
            brakes = 300;
        }
        else
        {
            brakes = 0;

        }


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            axleInfo.leftWheel.brakeTorque = brakes;
        }
    }
}
