using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public GameObject[] targets;
    public GameObject container;
    public GameObject CharacterSelect;
    public GameObject TeamSelect;
    public GameObject SpawnSelect;
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
            MouseClick(Input.mousePosition);
        }
    }

    public void ControllerClick(Vector3 pos)
    {
        Debug.Log("Spawning at: " + pos);
        RaycastHit hitInfo;
        if (Physics.Raycast(pos, new Vector3(pos.x, -pos.y, pos.z), out hitInfo))
        {
            Debug.Log(hitInfo.transform.tag + " from ray at" + hitInfo.transform.position);
            if (hitInfo.transform.tag == "Ground")
            {
                SpawnPlayer(new Vector3(hitInfo.point.x, hitInfo.point.y + m_SpawnHeight, hitInfo.point.z), transform.rotation);
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
        CharacterSelect.SetActive(false);

    }

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        container.transform.position = position;
        container.transform.rotation = rotation;
        transform.rotation = rotation;

        targets[(int)m_Selection].transform.position = position;

        targets[(int)m_Selection].SetActive(true);
        Follow(targets[(int)m_Selection].transform);
        

    }

    public void Follow(Transform parent)
    {
        transform.parent = parent;
        transform.Rotate(m_FollowXRot, 0,0);
        transform.localPosition = m_FollowPosition;
        offset = parent.position - transform.position;
        Debug.Log("Setting cam pos");
        VehicleSelected = false;
    }

    public void MenuCamera()
    {
        transform.parent = container.transform;
        transform.rotation = Quaternion.identity;
        transform.Rotate(90.0f, 90.0f, 0.0f);
        transform.position = m_MenuPosition;
        CharacterSelect.SetActive(true);
        TeamSelect.SetActive(false);
    }

    public void TeamCamera()
    {
        MenuCamera();
        CharacterSelect.SetActive(false);
        TeamSelect.SetActive(true);
    }
}