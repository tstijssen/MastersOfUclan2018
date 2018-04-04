﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public GameObject[] targets;
    public GameObject container;
    public GameObject selection_Panel;
    public Vector3 m_MenuPosition;

    public Vector3 m_FollowPosition;
    public float m_FollowXRot;
    public float m_SpawnHeight = 10.0f;
    Vector3 offset;

    public FireType m_Selection;
    bool VehicleSelected;
    Camera m_Camera;

    void Start()
    {
        VehicleSelected = false;
        MenuCamera();
        m_Camera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        if (!VehicleSelected && targets[(int)m_Selection].GetComponent<CarFireControl>().m_Despawned)
        {
            MenuCamera();
            VehicleSelected = false;
            targets[(int)m_Selection].SetActive(false);
            targets[(int)m_Selection].GetComponent<CarFireControl>().m_Despawned = false;
        }

        if (Input.GetMouseButtonDown(0) && VehicleSelected)
        {
            //Vector3 mousePos = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_Camera.transform.position.y + m_Camera.nearClipPlane));
            //Debug.Log(mousePos.ToString());
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.transform.tag);
                if (hitInfo.transform.tag == "Ground")
                {
                    SpawnPlayer(new Vector3(hitInfo.point.x, hitInfo.point.y + m_SpawnHeight, hitInfo.point.z));
                }
            }
        }
    }

    public void SetSelection(int type)
    {
        m_Selection = (FireType)type;
        VehicleSelected = true;
        selection_Panel.SetActive(false);

    }

    public void SpawnPlayer(Vector3 position)
    {
        container.transform.position = position;
        targets[(int)m_Selection].transform.position = position;
        targets[(int)m_Selection].SetActive(true);
        Follow(targets[(int)m_Selection].transform);

        //switch (m_Selection)
        //{
        //    case FireType.TwinGuns:
        //        targets[0].SetActive(true);
        //        Follow(targets[0].transform);
        //        break;

        //    case FireType.Beam:
        //        targets[1].SetActive(true);
        //        Follow(targets[1].transform);
        //        break;

        //    case FireType.Ram:
        //        targets[2].SetActive(true);
        //        Follow(targets[2].transform);
        //        break;

        //    case FireType.Cannon:
        //        targets[3].SetActive(true);
        //        Follow(targets[3].transform);
        //        break;
        //}
    }

    public void Follow(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = m_FollowPosition;
        transform.rotation = Quaternion.identity;
        transform.Rotate(m_FollowXRot, 0.0f, 0.0f);
        offset = parent.position - transform.position;
        Debug.Log("Setting cam pos");
        VehicleSelected = false;
    }

    public void MenuCamera()
    {
        transform.parent = null;
        transform.rotation = Quaternion.identity;
        transform.Rotate(90.0f, 90.0f, 0.0f);
        transform.position = m_MenuPosition;
        selection_Panel.SetActive(true);
    }

}