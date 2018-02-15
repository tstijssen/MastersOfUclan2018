using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTravel : MonoBehaviour {

    public float m_BulletSpeed; 
    public float m_BulletLife;  // timer until bullet sel-deactivates
    public float m_ReloadSpeed; // accessed by tank when shooting bullet, used to set reload timer
    private float m_LifeReset;  // used to reset bulletLife var when returning bullet to object pool
    //private TeamNames m_Team;   // identifies team of the player that shot this bullet instance
    public GameObject Splode;

    // Use this for initialization
    void Start () {
        m_LifeReset = m_BulletLife;

    }

    //public void SetTeam(TeamNames newTeam)
    //{
    //    m_Team = newTeam;
    //}

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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TankLocal>().TakeDamage();
            ResetBullet();
        }
        else
        {
            ResetBullet();
        }
    }

    // reset bullet life counter and return it to the object pool for reuse
    private void ResetBullet()
    {
        Splode.transform.position = transform.position;
        Instantiate(Splode);        
        m_BulletLife = m_LifeReset;
        this.gameObject.SetActive(false);
    }
    
}
