using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour {

    public float SpinSpeed = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(SpinSpeed * Time.deltaTime,SpinSpeed * Time.deltaTime,0);
	}

}
