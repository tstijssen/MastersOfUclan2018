using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallTravel : MonoBehaviour {

    public float m_Damage;
    public float m_Speed;
    public float m_LifeTime;
    public bool m_Active;
    private float m_LifeReset;  // used to reset bulletLife var when returning bullet to object pool
    Rigidbody rb;
    public CarFireControl m_Owner;

    public Vector3 direction;
    // Use this for initialization
    void Start () {
        m_LifeReset = m_LifeTime;
        rb = GetComponent<Rigidbody>();
        m_Active = false;

    }

    private void OnTriggerExit(Collider other)
    {
        // exited player's collision space, bullet now active
        if (!m_Active && (other.tag == "Player" || other.tag == "Player 1" || other.tag == "Player 2"))
        {
            m_Active = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        // move bullet and decrease life counter
        //rb.AddForce((direction * (m_Speed)) * 10);
        transform.localPosition += direction * (m_Speed / 100);

    }

    void Update()
    {
        m_LifeTime -= Time.deltaTime;

        if (m_LifeTime < 0)
        {
            ResetBullet();
        }
    }

    public void ResetBullet()
    {
        m_LifeTime = m_LifeReset;
        m_Active = false;
        gameObject.SetActive(false);
        Debug.Log(gameObject.activeInHierarchy);
    }
}
