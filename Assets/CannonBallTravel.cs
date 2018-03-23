using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallTravel : MonoBehaviour {


    public float m_Speed;
    public float m_LifeTime;
    private float m_LifeReset;  // used to reset bulletLife var when returning bullet to object pool

    // Use this for initialization
    void Start () {
        m_LifeReset = m_LifeTime;
	}
	
	// Update is called once per frame
	void Update () {
        // move bullet and decrease life counter
        transform.localPosition += transform.forward * m_Speed * Time.deltaTime;
        m_LifeTime -= Time.deltaTime;

        if (m_LifeTime < 0)
        {
            ResetBullet();
        }
    }

    public void ResetBullet()
    {
        m_LifeTime = m_LifeReset;
        //m_Active = false;
        gameObject.SetActive(false);
        Debug.Log(gameObject.activeInHierarchy);
    }
}
