using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum FireType {TwinGuns, Beam, Cannon };

public class CarFireControl : MonoBehaviour {

    public FireType gunType;

    public GameObject m_Barrel1;        // location for spawning shot projectiles
    public GameObject m_Barrel2;        // location for spawning shot projectiles
    private CarPooler m_BulletPool;

    public GameObject m_BeamBarrel;

    public bool altguns = false;
    public bool fired = false;
    private float m_ReloadTimer;       // counts down to 0, tank can only shoot when not counting down
    public float ReloadTwinGuns = 0.1f;
    public float ReloadBeamGun = 2.0f;
    public float BeamTime = 2.0f;

    public void Shoot()
    {
        //Shooting
        if (!fired)
        {
            if(gunType == FireType.TwinGuns)
            {
                GameObject bullet = m_BulletPool.GetPooledObject();  // get bullet object from pool
                if (bullet != null) // check if object pool returned a bullet
                {
                    if (m_ReloadTimer <= 0.0f)    // only shoot if not waiting for reload
                    {
                        Debug.Log("Shooting");

                        if (altguns)
                        {
                            m_Barrel1.GetComponent<AudioSource>().Play();
                            bullet.transform.position = m_Barrel1.transform.position;
                            bullet.transform.rotation = m_Barrel1.transform.rotation;
                            altguns = !altguns;
                        }
                        else
                        {
                            m_Barrel2.GetComponent<AudioSource>().Play();
                            bullet.transform.position = m_Barrel2.transform.position;
                            bullet.transform.rotation = m_Barrel2.transform.rotation;
                            altguns = !altguns;
                        }

                        fired = true;
                        bullet.SetActive(true);
                        m_ReloadTimer = ReloadTwinGuns;   // reset reload speed
                    }
                }
            }
            // solid beam from front of vehicle
            if (gunType == FireType.Beam)
            {
                GameObject beam = m_BulletPool.GetPooledObject();  // get bullet object from pool
                if (beam != null) // check if object pool returned a bullet
                {
                    if (m_ReloadTimer <= 0.0f)    // only shoot if not waiting for reload
                    {
                        Debug.Log("Shooting");
                        m_BeamBarrel.SetActive(true);
                        m_BeamBarrel.GetComponent<AudioSource>().Play();
                        //beam.transform.position = m_BeamBarrel.transform.position;
                        //beam.transform.rotation = m_BeamBarrel.transform.rotation;

                        //beam.SetActive(true);
                        fired = true;

                        m_ReloadTimer = BeamTime;   // reset reload speed
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {

            other.gameObject.SetActive(false);
        }
    }


    // Use this for initialization
    void Start () {
        m_ReloadTimer = 0.0f;
        m_BulletPool = GetComponent<CarPooler>();
	}

    private void Update()
    {
        //Reloading
        if (m_ReloadTimer > 0.0f)
        {
            m_ReloadTimer -= Time.deltaTime;
        }
        else
        {
            if (gunType == FireType.Beam && fired)
            {
                m_ReloadTimer = ReloadBeamGun;
                m_BeamBarrel.SetActive(false);
                fired = false;
                return;
            }
            m_ReloadTimer = 0.0f;
            fired = false;
        }
    }
}
