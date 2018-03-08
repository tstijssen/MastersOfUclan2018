using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Physics.GetIgnoreLayerCollision(gameObject.layer, 8);
	}
}
