using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum FireType {TwinGuns, Beam, Cannon };

public class CarFireControl : MonoBehaviour {

    public FireType gunType;

    public GameObject m_Barrel1;        // location for spawning shot projectiles
    public GameObject m_Barrel2;        // location for spawning shot projectiles
    private CarPooler m_BulletPool;

    public Slider m_HealthSlider;
    public Image m_SliderFillImage;

    public GameObject m_BeamBarrel;
    public GameObject m_Shield;
    public Transform m_Spawnpoint;
    public float m_Health = 100;

    public bool altguns = false;
    public bool fired = false;
    private float m_ReloadTimer;       // counts down to 0, tank can only shoot when not counting down

    private bool m_ShieldPowerUp;
    private float m_ShieldHealth;

    private bool m_ReloadPowerUp;
    private float m_ReloadMultiplier;     // used by powerups to speed up reloading
    private float m_ReloadPowerUpTimer;

    public float ReloadTwinGuns = 0.1f;
    public float ReloadBeamGun = 2.0f;
    public float BeamTime = 2.0f;

    public void Shoot()
    {
        //Shooting
        if (!fired)
        {
            if (gunType == FireType.TwinGuns)
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
                if (m_ReloadTimer <= 0.0f)    // only shoot if not waiting for reload
                {
                    Debug.Log("Shooting");
                    m_BeamBarrel.SetActive(true);
                    m_BeamBarrel.GetComponent<AudioSource>().Play();
                    fired = true;

                    m_ReloadTimer = BeamTime;   // reset reload speed       
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup" && other.GetComponent<PickupBehaviour>().ColliderTag != "Beam")   // hack to ensure beam weapon doesn't pick up objects
        {
            other.GetComponent<AudioSource>().Play();   // play pickup noise
            PickupBehaviour pickup = other.GetComponent<PickupBehaviour>();

            switch (pickup.Type)
            {
                case PickupType.Health:
                    m_Health += pickup.Value;
                    Debug.Log("HP picked up!");

                    break;

                case PickupType.Shield:
                    m_Shield.SetActive(true);
                    m_SliderFillImage.color = Color.cyan;
                    m_ShieldHealth = pickup.Value;
                    m_ShieldPowerUp = true;
                    Debug.Log("Shield picked up!");

                    break;

                case PickupType.PowerUp:
                    Debug.Log("Powerup picked up!");

                    //??
                    break;

                case PickupType.ReloadSpeed:
                    m_ReloadPowerUp = true;
                    m_ReloadMultiplier = pickup.Value;
                    m_ReloadPowerUpTimer = pickup.Duration;
                    Debug.Log("Reload up picked up!");

                    break;
            }
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Bullet")
        {
            if (other.GetComponent<BulletTravel>().m_Active)
            {
                if (m_ShieldPowerUp)
                {
                    m_ShieldHealth -= other.GetComponent<BulletTravel>().m_Damage;
                }
                else
                {
                    m_Health -= other.GetComponent<BulletTravel>().m_Damage;
                }
                Debug.Log("bullethit");
                other.GetComponent<BulletTravel>().ResetBullet();

            }
        }
        else if(other.tag == "Hazard")
        {
            m_ShieldHealth = 0.0f;
            m_Health = 0.0f;
            m_ReloadPowerUpTimer = 0.0f;

            Respawn();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Beam")
        {
            if (m_ShieldPowerUp)
            {
                m_ShieldHealth -= (other.GetComponent<laserScript>().m_Damage * Time.deltaTime);
            }
            else
            {
                m_Health -= (other.GetComponent<laserScript>().m_Damage * Time.deltaTime);
            }
        }
    }

    // Use this for initialization
    void Start () {
        m_ReloadTimer = 0.0f;
        m_BulletPool = GetComponent<CarPooler>();
        m_SliderFillImage.color = Color.green;

    }

    private void Respawn()
    {
        m_Health = 100.0f;
        transform.position = m_Spawnpoint.position;
    }

    private void Update()
    {
        // went off edge
        if(transform.position.y < -1.0f || m_Health <= 0.0f)
        {
            m_ShieldHealth = 0.0f;
            m_Health = 0.0f;
            m_ReloadPowerUpTimer = 0.0f;

            Respawn();
        }

        // update health slider
        if(m_ShieldPowerUp)
        {
            m_HealthSlider.value = m_ShieldHealth;
            if (m_ShieldHealth <= 0.0f)
            {
                Debug.Log("Deactivate Shield!");
                m_Shield.SetActive(false);
                m_SliderFillImage.color = Color.green;
                m_ShieldPowerUp = false;
            }
        }
        else
            m_HealthSlider.value = m_Health;

        // power up timer
        if (m_ReloadPowerUp)
        {
            m_ReloadPowerUpTimer -= Time.deltaTime;
            if(m_ReloadPowerUpTimer <= 0.0f)
            {
                m_ReloadPowerUp = false;
            }
        }

        //Reloading
        if (m_ReloadTimer > 0.0f)
        {
            if (m_ReloadPowerUp)
                m_ReloadTimer -= Time.deltaTime * m_ReloadMultiplier;
            else
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
