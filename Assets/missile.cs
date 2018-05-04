using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour {

    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed = 20f;
    public GameObject turret;
    Quaternion startRot;
    public GameObject explosion;
    

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    float speed;

    // Use this for initialization
    void Start ()
    {
        startRot = transform.rotation;
	}

    private void OnEnable()
    {
        transform.rotation = new Quaternion(startRot.x, turret.transform.rotation.y, startRot.z, startRot.w);
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameObject.tag == "missile")
        {
            speed = 10f;

            //find the vector pointing from our position to the target
            _direction = (Target.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
        else
        {

            speed = 50f;
            transform.LookAt(Target);
        }



        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            Debug.Log("Hit");
            turret.GetComponent<TurretFire>().fired = false;
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            transform.rotation = startRot;
            gameObject.SetActive(false);

        }
    }
}

