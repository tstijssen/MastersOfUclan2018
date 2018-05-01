using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class ControllerMouse : MonoBehaviour {
    float h, v;
    bool pressed;
    bool toggle;

    public GameObject [] targets;
    public FollowCamera cam;
    public GameObject container;
    public bool spawnSelect = false;
    public string MoveCommand = "Horizontal";
    public string ClickCommand = "Fire1";
    int selectedTarget;
    GamePadState gamePad;

    bool canInteract;

    // Use this for initialization
    void Start () {

        h = 0.0f;
        v = 0.0f;
        selectedTarget = -1;

        canInteract = false;
        pressed = false;
        toggle = false;
        StartCoroutine(MenuChange());
        selectedTarget = 0;

        if (spawnSelect)
        {
            targets = GameObject.FindGameObjectsWithTag("Spawn");
        }
    }

    void OnEnable()
    {
        canInteract = false;
        pressed = false;
        toggle = false;
        StartCoroutine(MenuChange());
        Debug.LogWarning("Enabling");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }

    // Update is called once per frame
    void Update()
    {
        gamePad = cam.GetComponentInParent<LocalPlayerSetup>().m_GamePadState;
        if (gamePad.IsConnected)
        {
            toggle = (gamePad.Buttons.B == ButtonState.Pressed);
            pressed = (gamePad.Buttons.A == ButtonState.Pressed);
        }
        else
        {
            h = Input.GetAxis(MoveCommand);
            pressed = Input.GetButton(ClickCommand);
        }


        if (toggle && canInteract)
        {
            selectedTarget = (selectedTarget + 1) % targets.Length;
            if (spawnSelect)
            {
                bool validSpawn = false;
                while (!validSpawn)
                {
                    if (targets[selectedTarget].GetComponent<SpawnData>().m_TeamNumber == (int)container.GetComponent<LocalPlayerSetup>().m_PlayerTeam
                    || targets[selectedTarget].GetComponent<SpawnData>().m_TeamNumber == -1)
                    {

                        validSpawn = true;
                        transform.position = new Vector3(targets[selectedTarget].transform.position.x, targets[selectedTarget].transform.position.y + 1.0f, targets[selectedTarget].transform.position.z);
                        cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z);
                    }
                    else
                    {
                        selectedTarget = (selectedTarget + 1) % targets.Length;
                    }
                }
            }
            else
                transform.position = targets[selectedTarget].transform.position;

            canInteract = false;
            StartCoroutine(MenuChange());
        }

        if (pressed && selectedTarget != -1 && canInteract)
        {
            if (spawnSelect)
                cam.GetComponent<FollowCamera>().SetSpawn(targets[selectedTarget].transform.position, targets[selectedTarget].transform.rotation);
            else
                targets[selectedTarget].GetComponent<Toggle>().onValueChanged.Invoke(true);
            pressed = false;
        }

    }

    IEnumerator MenuChange()
    {
        Debug.Log("Delaying");
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
    }

}
