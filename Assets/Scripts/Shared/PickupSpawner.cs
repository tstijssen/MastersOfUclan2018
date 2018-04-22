using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public GameObject m_Pickup;
    public float m_RespawnDuration;

    float m_Timer;
    bool m_Enabled;

	// Use this for initialization
	void Start () {
        m_Timer = 0.0f;
        m_Enabled = true;
    }

    // Update is called once per frame
    void Update () {
		
        if(m_Enabled && !m_Pickup.activeInHierarchy)
        {
            m_Timer = m_RespawnDuration;
            m_Enabled = false;
        }

        if (!m_Enabled)
        {
            m_Timer -= Time.deltaTime;

            if (m_Timer <= 0.0f)
            {
                m_Timer = 0.0f;
                m_Enabled = true;
                m_Pickup.SetActive(true);
            }
        }
	}
}
