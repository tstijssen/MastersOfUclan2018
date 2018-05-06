using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCaptureScript : MonoBehaviour {

    public FlagState m_State;
    public GameObject m_ScoreText;
    Vector3 m_HomePos;
    Quaternion m_HomeRot;
    public int m_VictoryNumber;
    OnlineFireControl m_OnlineOwner;
    CarFireControl m_LocalOwner;

    // Use this for initialization
    void Start () {
        m_HomePos = this.transform.position;
        m_HomeRot = this.transform.rotation;
        m_State = FlagState.Home;
        m_VictoryNumber = PlayerPrefs.GetInt("HATScoreLimit");
    }
	
	// Update is called once per frame
	void Update () {
        if(m_State != FlagState.Taken)
        {
            transform.Rotate(transform.up * 10.0f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CarFireControl car = other.GetComponent<CarFireControl>();
            if (car && car.m_Alive)
            {
                if (m_State != FlagState.Taken)
                {
                    TakeHat(car);
                }
            }
            else
            {
                OnlineFireControl onlineCar = other.GetComponent<OnlineFireControl>();
                if(onlineCar && onlineCar.m_Alive)
                {
                    if (m_State != FlagState.Taken)
                    {
                        TakeHat(onlineCar);
                    }
                }
            }
        }
        else if (other.tag == "Hazard")
        {
            transform.parent = null;
            ResetHat();
        }
    }

    private void TakeHat(OnlineFireControl onlinePlayer)
    {
        Debug.Log("taking flag");
        onlinePlayer.HatTaken(this);
        transform.parent = onlinePlayer.transform;
        m_OnlineOwner = onlinePlayer;
        m_State = FlagState.Taken;
    }

    private void TakeHat(CarFireControl player)
    {
        Debug.Log("taking flag");
        player.HatTaken(this);
        transform.parent = player.transform;
        m_LocalOwner = player;
        m_State = FlagState.Taken;
    }
    public void DropHat()
    {
        Debug.Log("Dropping flag");
        m_State = FlagState.Dropped;
        transform.parent = null;
    }

    public void ResetHat()
    {
        Debug.Log("resetting hat");
        m_State = FlagState.Home;
        this.transform.position = m_HomePos;
        this.transform.rotation = m_HomeRot;
    }

}
