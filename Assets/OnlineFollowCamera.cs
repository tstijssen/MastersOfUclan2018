using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineFollowCamera : MonoBehaviour {
    public GameObject[] targets;
    public GameObject container;
    public GameObject SpawnSelect;
    public Vector3 m_MenuPosition;

    public Vector3 m_FollowPosition;
    public float m_FollowXRot;
    public float m_SpawnHeight = 10.0f;
    Vector3 offset;

    public FireType m_Selection;
    bool VehicleSelected;
    Camera m_Camera;
    float m_RestPos = 0.0f;

    void Start()
    {
        VehicleSelected = true;
        m_Camera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        if (!VehicleSelected && container.GetComponent<OnlineFireControl>().m_Despawned)
        {
            MenuCamera();
            VehicleSelected = false;
            targets[(int)m_Selection].SetActive(false);
            container.GetComponent<Rigidbody>().useGravity = false;
            container.GetComponent<OnlineFireControl>().m_Despawned = false;
            container.transform.position = m_MenuPosition;
        }
        else if (targets[(int)m_Selection].activeInHierarchy)
        {
            transform.LookAt(container.transform);
            
        }
        else if (container != null && container.GetComponent<OnlineFireControl>().m_Alive)
        {
            targets[(int)m_Selection].SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && VehicleSelected)
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.transform.tag);
                if (hitInfo.transform.tag == "Spawn" && hitInfo.transform.GetComponent<SpawnData>().m_TeamNumber == -1 || hitInfo.transform.GetComponent<SpawnData>().m_TeamNumber == (int)container.GetComponent<OnlineFireControl>().m_PlayerTeam)
                {
                    m_RestPos = hitInfo.point.y + m_SpawnHeight / 10.0f;
                    SpawnPlayer(new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y + m_SpawnHeight, hitInfo.transform.position.z), hitInfo.transform.rotation);
                    SpawnSelect.SetActive(false);
                }
            }
        }
    }

    public void SetSpawn(Vector3 pos, Quaternion rot)
    {
        SpawnPlayer(new Vector3(pos.x, pos.y + m_SpawnHeight, pos.z), rot);
        SpawnSelect.SetActive(false);
    }

    public void MouseClick(Vector3 pos)
    {
        Debug.Log("Spawning at: " + pos);
        Ray ray = m_Camera.ScreenPointToRay(pos);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.transform.tag + " from ray at" + hitInfo.transform.position);
            if (hitInfo.transform.tag == "Ground")
            {
                SpawnPlayer(new Vector3(hitInfo.point.x, hitInfo.point.y + m_SpawnHeight, hitInfo.point.z), transform.rotation);
            }
        }
    }


    public void SetSelection(int type)
    {
        m_Selection = (FireType)type;
        VehicleSelected = true;
    }

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        container.transform.position = position;
        container.transform.rotation = Quaternion.identity;
        container.transform.rotation = rotation;
        targets[(int)m_Selection].transform.position = position;
        targets[(int)m_Selection].transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        container.GetComponent<Rigidbody>().useGravity = true;
        container.GetComponent<OnlineFireControl>().m_Alive = true;

        targets[(int)m_Selection].SetActive(true);
        Follow(targets[(int)m_Selection].transform);
    }

    public void Follow(Transform parent)
    {
        transform.parent = parent;
        transform.Rotate(m_FollowXRot, 0, 0);
        transform.localPosition = m_FollowPosition;

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
        SpawnSelect.SetActive(true);
    }

}