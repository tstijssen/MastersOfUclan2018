using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {

    public ObjectPooler[] m_PickupPools;
    public float m_SpawnDelay;

    private float m_SpawnTimer;

	// Use this for initialization
	void Start () {
        m_SpawnTimer = m_SpawnDelay;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_SpawnTimer > 0.0f)
        {
            m_SpawnTimer -= Time.deltaTime;
        }
        else
        {
            m_SpawnTimer = m_SpawnDelay;
            // spawn pickup
            float xPos = Random.Range(-33.0f, 33.0f);
            float zPos = Random.Range(-33.0f, 33.0f);
            float yPos = 1.0f;

            int type = Random.Range(0, 2);

            GameObject newPickup = m_PickupPools[type].GetPooledObject();

            if(newPickup != null)
            {
                newPickup.transform.position = new Vector3(xPos, yPos, zPos );
                newPickup.SetActive(true);
            }
        }
    }
}
