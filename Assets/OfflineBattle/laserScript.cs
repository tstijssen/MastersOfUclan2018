﻿using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {

    public float m_Damage;    // continuous, as long as inside beam, divided by frametime
    public float m_Width;
    public Transform startPoint;
	public Transform endPoint;
    private Transform linecastedTransform;
	LineRenderer laserLine;

	// Use this for initialization
	void OnEnable () {
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hitInfo;
        linecastedTransform = endPoint;

        if (Physics.Linecast(startPoint.transform.position, endPoint.transform.position, out hitInfo))
        {
            Debug.Log("Before " + linecastedTransform.position);
                linecastedTransform.position.Set(hitInfo.transform.position.x, linecastedTransform.position.y, hitInfo.transform.position.z);
            Debug.Log("After " + linecastedTransform.position);
        }

        laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.startWidth = m_Width;
        laserLine.endWidth = m_Width;

        laserLine.SetPosition (0, startPoint.position);
        laserLine.SetPosition(1, linecastedTransform.position);
    }
}