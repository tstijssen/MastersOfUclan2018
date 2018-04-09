using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineFollowCamera : MonoBehaviour {
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
    bool m_Grounded;
    Camera m_Camera;
    float m_RestPos = 0.0f;

    void Start()
    {
        VehicleSelected = false;
        MenuCamera();
        m_Camera = this.GetComponent<Camera>();
        m_Grounded = false;
    }

    private void Update()
    {
        if (!VehicleSelected && container.GetComponent<OnlineFireControl>().m_Despawned)
        {
            MenuCamera();
            VehicleSelected = false;
            targets[(int)m_Selection].SetActive(false);
            container.GetComponent<OnlineFireControl>().m_Despawned = false;
            container.transform.position = m_MenuPosition;
            container.GetComponent<Rigidbody>().useGravity = false;
            m_Grounded = false;
        }

        if (!m_Grounded && !VehicleSelected && container.GetComponent<OnlineFireControl>().m_Alive && container.transform.position.y <= m_RestPos)
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(m_FollowXRot, 0.0f, 0.0f);
            transform.localPosition = m_FollowPosition;
            container.transform.position = new Vector3(container.transform.position.x, 1.0f, container.transform.position.z);
            m_Grounded = true;
        }

        //if (m_Grounded)
        //{
        //    transform.LookAt(targets[(int)m_Selection].transform);
        //}

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
                    m_RestPos = hitInfo.point.y + m_SpawnHeight / 10.0f;
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
        //targets[(int)m_Selection].transform.position = position;
        targets[(int)m_Selection].SetActive(true);
        Follow(targets[(int)m_Selection].transform);
        container.GetComponent<OnlineFireControl>().m_GunData.gunType = m_Selection;

        if (m_Selection == FireType.Beam)
        {
            container.GetComponent<UnityStandardAssets.Vehicles.Car.OnlineUserControl>().enabled = false;
            container.GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().enabled = false;

            container.GetComponent<OnlineHoverMovement>().enabled = true;
            container.GetComponent<Rigidbody>().useGravity = false;
            container.GetComponent<Rigidbody>().mass = 70;
            container.GetComponent<Rigidbody>().drag = 3;
            container.GetComponent<Rigidbody>().angularDrag = 4;
        }
        else
        {
            container.GetComponent<UnityStandardAssets.Vehicles.Car.OnlineUserControl>().enabled = true;
            container.GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().enabled = true;

            container.GetComponent<OnlineHoverMovement>().enabled = false;
            container.GetComponent<Rigidbody>().useGravity = true;
            container.GetComponent<Rigidbody>().mass = 1000;
            container.GetComponent<Rigidbody>().drag = 0.1f;
            container.GetComponent<Rigidbody>().angularDrag = 0.05f;
        }
    }

    public void Follow(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = new Vector3(0, 100, 0);

        container.GetComponent<Rigidbody>().useGravity = true;
        offset = parent.position - transform.position;
        Debug.Log("Setting cam pos");
        VehicleSelected = false;
    }

    public void MenuCamera()
    {
        transform.parent = null;
        transform.rotation = Quaternion.identity;
        transform.Rotate(90.0f, -90.0f, 0.0f);
        transform.position = m_MenuPosition;
        selection_Panel.SetActive(true);
    }

}