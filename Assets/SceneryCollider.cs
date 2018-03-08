using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit from: " + other.tag);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
