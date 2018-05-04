using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawn : MonoBehaviour {

    public GameObject ai1;
    public GameObject ai2;
    public GameObject ai3;

    public GameObject ai1Tele;
    public GameObject ai2Tele;
    public GameObject ai3Tele;

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
        if(!ai1.activeInHierarchy)
        {
            timer1 += Time.deltaTime;
            Debug.Log(timer1);
            if (timer1 > 7f)
            {
                Debug.Log("SPawning");
                ai1Tele.transform.position = ai1Pos;
                ai1Tele.SetActive(true);   
                ai1.transform.position = ai1Pos;
                ai1.SetActive(true);
                timer1 = 0;
            }
        }
        if (!ai2.activeInHierarchy)
        {
            timer2 += Time.deltaTime;
            Debug.Log(timer2);
            if (timer2 > 3f)
            {
                Debug.Log("SPawning");
                ai2Tele.transform.position = ai2Pos;
                ai2Tele.SetActive(true);
                ai2.transform.position = ai2Pos;
                ai2.SetActive(true);
                timer2 = 0;      
            }
        }
        if (!ai3.activeInHierarchy)
        {
            timer3 += Time.deltaTime;
            Debug.Log(timer3);
            if (timer3 > 3f)
            {
                Debug.Log("SPawning");
                ai3Tele.transform.position = ai3Pos;
                ai3Tele.SetActive(true);
                ai3.transform.position = ai3Pos;
                ai3.SetActive(true);
                timer3 = 0;
               
            };
        }
    }
}
