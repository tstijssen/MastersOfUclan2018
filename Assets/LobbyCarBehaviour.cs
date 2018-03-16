using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarBehaviour : MonoBehaviour {

    public float SpinSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, SpinSpeed * Time.deltaTime, 0);
	}
}
