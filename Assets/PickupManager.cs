using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {

    public ObjectPooler[] m_PickupPools;
    public float m_SpawnDelay;

    private float m_SpawnTimer;
    Vector3 targetPos;
    bool validSpawn;
    // Use this for initialization
    void Start () {
        m_SpawnTimer = m_SpawnDelay;
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
            GetRandomPos();
            m_SpawnTimer = m_SpawnDelay;
            int type = Random.Range(0, 3);

            GameObject newPickup = m_PickupPools[type].GetPooledObject();

            if (newPickup != null)
            {
                newPickup.transform.position = targetPos;
                newPickup.SetActive(true);
            }
            //RaycastHit hitInfo;
            //if (Physics.Linecast(new Vector3(targetPos.x, 5.0f, targetPos.z), targetPos, out hitInfo))
            //{
            //    Debug.Log(hitInfo.transform.tag);
            //    if(hitInfo.transform.tag == "Ground")
            //    {
            //        validSpawn = true;
            //    }
            //}
            //if(validSpawn)
            //{

            //}
        }
    }
}
