using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseDamage : MonoBehaviour {
    public Slider health;
    public float hp;
    public Image hpImage;

    public GameObject endgameScreen;

	// Use this for initialization
	void Start ()
    {
       // hp = health.maxValue;
        //health.value = hp;
    }
	
	// Update is called once per frame
	void Update () {

        //health.value = hp;
        hpImage.fillAmount = hp;
        //Debug.Log(hp);
        //Debug.Log(health.value);
        if(hp < 0f)
        {

            Debug.Log("game end");
            Time.timeScale = 0.01f;
            endgameScreen.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "missile")
        {
            hp -= 0.05f;
            //health.value -= 5;
        }
        else if(other.gameObject.tag == "shell")
        {
            //health.value -= 1;
            hp -= 0.01f;

        }

    }

}
