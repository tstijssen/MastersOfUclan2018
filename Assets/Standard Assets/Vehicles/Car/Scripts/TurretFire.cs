using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFire : MonoBehaviour {

    public bool isDriving;
    public GameObject missile;
    public bool fired = false;

    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        isDriving = GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarAIControl>().m_Driving;

        if (!isDriving)
        {
            if (gameObject.tag == "missile")
            {
                missile.SetActive(true);
                if(!fired)
                {
                missile.transform.position = transform.position + Vector3.up;
                    fired = true;
                }
            }
            else
            {
                //find the vector pointing from our position to the target
                _direction = (Target.position - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

            }
        }
    


		
	}
}
