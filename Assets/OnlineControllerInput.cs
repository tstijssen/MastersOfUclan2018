using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XInputDotNetPure;

public class OnlineControllerInput : MonoBehaviour {

    GamePadState gamePad;
    public GameObject[] targets;
    public GameObject container;
    public OnlineFollowCamera cam;
    bool canInteract;
    [HideInInspector]
    public bool pressed;
    [HideInInspector]
    public bool toggle;
    int selectedTarget;

    // Use this for initialization
    void Start () {
        canInteract = false;
        pressed = false;
        toggle = false;
        StartCoroutine(MenuChange());
        targets = GameObject.FindGameObjectsWithTag("Spawn");
        selectedTarget = 0;
    }

    // Update is called once per frame
    void Update () {
        if (toggle && canInteract)
        {
            selectedTarget = (selectedTarget + 1) % targets.Length;
            bool validSpawn = false;
            while (!validSpawn)
            {
                if (targets[selectedTarget].GetComponent<SpawnData>().m_TeamNumber == (int)container.GetComponent<OnlineFireControl>().m_PlayerTeam
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

            canInteract = false;
            StartCoroutine(MenuChange());
        }


    }

    public void CycleSelection()
    {
        if (canInteract)
        {
            Debug.Log("Cycling");
            selectedTarget = (selectedTarget + 1) % targets.Length;
            bool validSpawn = false;
            while (!validSpawn)
            {
                if (targets[selectedTarget].GetComponent<SpawnData>().m_TeamNumber == (int)container.GetComponent<OnlineFireControl>().m_PlayerTeam
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

            canInteract = false;
            StartCoroutine(MenuChange());
        }
    }

    public void SelectionPressed()
    {
        if (selectedTarget != -1 && canInteract)
        {
            cam.SetSpawn(targets[selectedTarget].transform.position, targets[selectedTarget].transform.rotation);
        }
    }

    IEnumerator MenuChange()
    {
        Debug.Log("Delaying");
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
    }

}
