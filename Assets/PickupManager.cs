using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {

    public ObjectPooler[] m_PickupPools;
    public float m_SpawnDelay;

    private float m_SpawnTimer;
    Vector3 targetPos;
    bool validSpawn;
    bool validPickup;
    GameObject newPickup;
    // Use this for initialization
    void Start () {
        m_SpawnTimer = m_SpawnDelay;
        validPickup = false;
    }
	
    void GetRandomPos()
    {
        validSpawn = false;
        // spawn pickup
        float xPos = Random.Range(-33.0f, 33.0f);
        float zPos = Random.Range(-33.0f, 33.0f);
        float yPos = 1;
        targetPos = new Vector3(xPos, yPos, zPos);
    }

	// Update is called once per frame
	void Update () {
        if (m_SpawnTimer > 0.0f)
        {
            m_SpawnTimer -= Time.deltaTime;
        }
        else
        {
            // get pickup from object pool, retry until one is obtained
            if(!validPickup)
            {
                int type = Random.Range(0, 3);
                newPickup = m_PickupPools[type].GetPooledObject();    
                if (newPickup != null)
                    validPickup = true;
            }

            // got pickup object, get random location and spawn
            if (validPickup)
            {
                GetRandomPos();
                Debug.Log("Got pickup obj");
                // raycast to ground to find empty spot to spawn, retry random positions until one is found
                RaycastHit hitInfo;
                if (Physics.Linecast(new Vector3(targetPos.x, 5.0f, targetPos.z), new Vector3(targetPos.x, -6.0f, targetPos.z), out hitInfo))
                {
                    Debug.Log(hitInfo.transform.tag);
                    if (hitInfo.transform.tag == "Ground")
                    {
                        validSpawn = true;
                    }
                }

                if (validSpawn)
                {
                    m_SpawnTimer = m_SpawnDelay;

                    newPickup.transform.position = targetPos;
                    newPickup.SetActive(true);
                    validPickup = false;
                }
            }
            
            
        }
    }
}
