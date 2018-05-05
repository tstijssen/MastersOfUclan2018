using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlatformRotate : MonoBehaviour {

    public float m_RotateSpeed;
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, 0, m_RotateSpeed * Time.deltaTime);
	}
}
