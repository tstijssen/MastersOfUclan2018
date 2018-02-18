using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextFaceCam : MonoBehaviour
{
    TextMesh P1;
     

	// Use this for initialization
	void Start () {
        P1 = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        P1.transform.LookAt(Camera.main.transform);
	}
}
