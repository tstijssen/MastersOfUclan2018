using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocker : MonoBehaviour {

    Renderer ren;
	// Use this for initialization
	void Start () {
        ren = GetComponent<Renderer>();
	}

    private void OnMouseEnter()
    {
        ren.material.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
    }

    private void OnMouseExit()
    {
        ren.material.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.0f);
    }
}
