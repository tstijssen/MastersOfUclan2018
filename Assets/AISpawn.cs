using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawn : MonoBehaviour {

    public GameObject ai1;
    public GameObject ai2;
    public GameObject ai3;

    Vector3 ai1Pos;
    Vector3 ai2Pos;
    Vector3 ai3Pos;

    float timer1 = 0f;
    float timer2 = 0f;
    float timer3 = 0f;

        float spawnTimer = 0;
    // Use this for initialization
    void Start ()
    {
        ai1Pos = ai1.transform.position;
        ai2Pos = ai2.transform.position;
        ai3Pos = ai3.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        Respawn(ai1, timer1, ai1Pos);
        Respawn(ai2, timer2, ai2Pos);
        Respawn(ai3, timer3, ai3Pos);
    }

    void Respawn(GameObject ai, float timer, Vector3 pos)
    {
        Debug.Log("Updating");
        if (!ai.activeInHierarchy)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            if(timer > 3f)
            {
            Debug.Log("SPawning");
                ai.transform.position = pos;
                ai.SetActive(true);
                timer = 0;
            }
            
        }
    }

}
