using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LocalFlagScript: MonoBehaviour
{
    public float m_ReturnTime;
    public TeamColours m_FlagColour;
    public GameObject m_FlagModel;
    public int m_VictoryNumber;
    Vector3 m_HomePos;
    Quaternion m_HomeRot;
    float m_Timer;
    public FlagState m_State;

    private void Start()
    {
        m_HomePos = this.transform.position;
        m_HomeRot = this.transform.rotation;
        m_State = FlagState.Home;
        m_VictoryNumber = PlayerPrefs.GetInt("CTF_FlagLimit");

        switch (m_FlagColour)
        {
            case TeamColours.Red:
                m_FlagModel.GetComponent<Renderer>().material.color = Color.red;
                break;
            case TeamColours.Blue:
                m_FlagModel.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case TeamColours.Yellow:
                m_FlagModel.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case TeamColours.Green:
                m_FlagModel.GetComponent<Renderer>().material.color = Color.green;
                break;
        }

    }

    private void Update()
    {
        if (m_State == FlagState.Dropped)
        {
            m_Timer -= Time.deltaTime;

            if (m_Timer < 0)
            {
                ResetFlag();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CarFireControl car = other.GetComponent<CarFireControl>();
            if (car.m_Alive)
            {
                if (m_State == FlagState.Home)
                {
                    if (car.m_PlayerTeam != m_FlagColour)
                    {
                        TakeFlag(car);
                    }
                }
                else if (m_State == FlagState.Dropped)
                {
                    if (car.m_PlayerTeam == m_FlagColour)
                    {
                        ResetFlag();
                    }
                    else
                    {
                        TakeFlag(car);
                    }
                }
            }
        }
        else if (other.tag == "Hazard")
        {
            transform.parent = null;
            ResetFlag();
        }
    }

    private void ResetFlag()
    {
        Debug.Log("resetting flag");
        m_State = FlagState.Home;
        this.transform.position = m_HomePos;
        this.transform.rotation = m_HomeRot;
    }

    private void TakeFlag(CarFireControl player)
    {
        Debug.Log("taking flag");
        player.FlagTaken(this);
        transform.parent = player.transform;
        m_State = FlagState.Taken;
    }

    public void CaptureFlag(TeamColours team)
    {
        Debug.Log("capturing flag");
        transform.parent = null;
        ResetFlag();
    }

    public void DropFlag()
    {
        Debug.Log("Dropping flag");
        m_State = FlagState.Dropped;
        transform.parent = null;
        m_Timer = m_ReturnTime;
    }

}
