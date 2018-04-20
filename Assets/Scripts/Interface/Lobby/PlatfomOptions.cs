using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlatfomOptions : MonoBehaviour {

    public GameObject[] car;// = new GameObject[3];
    public GamePadState gamePad;


    float speed = 10.0f;
    public bool isReady = false;
	bool canInteract;

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
		canInteract = false;
		StartCoroutine (MenuChange());
    }

    private void OnEnable()
    {
        isReady = false;
    }

    // Update is called once per frame
    void Update ()
    {
        
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

			if (canInteract && gamePad.ThumbSticks.Left.X < -0.5f)
			{
				DecChoice ();
				canInteract = false;
				StartCoroutine (MenuChange());
			}

			if (canInteract && gamePad.ThumbSticks.Left.X > 0.5f)
			{
				IncChoice ();
				canInteract = false;
				StartCoroutine (MenuChange());
			}

			if (canInteract && gamePad.Buttons.A == ButtonState.Pressed)
			{
				PickReady ();
				canInteract = false;
				StartCoroutine (MenuChange());
			}
        }
    }

    void PickReady()
    {
        //switch (gameObject.name)
        //{
        //    case "Platform1":
        //        break;
        //    case "Platform2":
        //        break;
        //    case "Platform3":
        //        break;
        //    case "Platform4":
        //        break;
        //}

        if(!isReady)
            GetComponentInParent<PlatformActivator>().noPlayers++;


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

	IEnumerator MenuChange()
	{
		Debug.Log("Delaying");
		yield return new WaitForSeconds(0.25f);
		canInteract = true;   // After the wait is over, the player can interact with the menu again.
	}
}
