using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject pausePanel;

    public Animator panelAnim;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                pausePanel.SetActive(true);
                if (panelAnim.GetCurrentAnimatorStateInfo(5).IsName("PauseMenu"))
                {

                }
            }
            else
            {
                panelAnim.speed = -1;
                if (panelAnim.GetCurrentAnimatorStateInfo(0).IsName("PauseMenu"))
                {


                }
            }
        }
    }

    public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        panelAnim.speed *= -1;
    }
}