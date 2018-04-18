using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlatfomOptions : MonoBehaviour {

    public GameObject[] car;// = new GameObject[3];
    GamePadState gamePad;


    float speed = 10.0f;
    public bool isReady = false;
    
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

    private void OnEnable()
    {
        isReady = false;
    }

    // Update is called once per frame
    void Update ()
    {
        //gamePad = container.GetComponent<MenuControllerDetect>().state;
        if (isReady)
        {

            //find the vector pointing from our position to the target
            _direction = (Target.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
        else
        {
            transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));


        }
    }

    void PickReady()
    {
        switch (gameObject.name)
        {
            case "Platform1":
                break;
            case "Platform2":
                break;
            case "Platform3":
                break;
            case "Platform4":
                break;
        }
        isReady = !isReady;
    }


    void IncChoice()
    {
        if (!isReady)
        {
            car[carPick].SetActive(false);
            carPick++;

            if (carPick > 3)
                carPick = 0;

            car[carPick].SetActive(true);
        }
    }

    void DecChoice()
    {
        if (!isReady)
        {
            car[carPick].SetActive(false);
            carPick--;

            if (carPick < 0)
                carPick = 3;

            car[carPick].SetActive(true);
        }
    }

}
