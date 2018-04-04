using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTravel : MonoBehaviour {

    public float m_Damage;    // once per bullet
    public float m_BulletSpeed; 
    public float m_BulletLife;  // timer until bullet sel-deactivates
    public float m_ReloadSpeed; // accessed by tank when shooting bullet, used to set reload timer
    private float m_LifeReset;  // used to reset bulletLife var when returning bullet to object pool
                                //private TeamNames m_Team;   // identifies team of the player that shot this bullet instance

    public CarFireControl m_Owner;
    public bool m_Active;
    bool m_Resetting;
    private GameObject ScoreInfo;

    private GameObject enemy;
    private TankLocal enemyscript;

    // Use this for initialization
    void Start () {
        m_LifeReset = m_BulletLife;
        m_Active = false;
        m_Resetting = false;
    }

    // Update is called once per frame
    void Update () {
        // move bullet and decrease life counter
        transform.localPosition += transform.forward * m_BulletSpeed * Time.deltaTime;
        m_BulletLife -= Time.deltaTime;

        if (m_BulletLife < 0)
        {
            ResetBullet();
        }
    }

    //private void OnEnable()
    //{
    //    m_Resetting = false;
    //}

    private void OnTriggerExit(Collider other)
    {
        // exited player's collision space, bullet now active
        if(!m_Active && (other.tag == "Player" || other.tag == "Player 1" || other.tag == "Player 2"))
        {
            Debug.Log("BulletActive");
            m_Active = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Scenery")
        {
            ResetBullet();
            Debug.Log("BulletReset");

        }
    }

    // reset bullet life counter and return it to the object pool for reuse
    public void ResetBullet()
    {
        m_BulletLife = m_LifeReset;
        m_Active = false;
        gameObject.SetActive(false);
        Debug.Log(gameObject.activeInHierarchy);
    }
    
}
