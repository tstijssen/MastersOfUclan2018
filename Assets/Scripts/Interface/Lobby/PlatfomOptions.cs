using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatfomOptions : MonoBehaviour {

    public GameObject[] car;// = new GameObject[3];

    bool isReady = false;

    public int carPick;

    public Button ready;
    public Button left;
    public Button right;


    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;


    // Use this for initialization
    void Start ()
    {
        left.onClick.AddListener(DecChoice);
        right.onClick.AddListener(IncChoice);
        ready.onClick.AddListener(PickReady);

        car[carPick].SetActive(true);

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(0f, 10f * Time.deltaTime, 0f));

        if(isReady)
        {
            //find the vector pointing from our position to the target
            _direction = (Target.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

        }



    }

    void PickReady()
    {

        isReady = true;
    }


    void IncChoice()
    {
        car[carPick].SetActive(false);
        carPick++;

        if (carPick > 3)
            carPick = 0;

        car[carPick].SetActive(true);
    }

    void DecChoice()
    {
        car[carPick].SetActive(false);
        carPick--;

        if (carPick < 0)
            carPick = 3;

        car[carPick].SetActive(true);
    }

}
