using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnlineFireControl : NetworkBehaviour {

    public CarData m_CarData;
    public GunData m_GunData;
    public HeatFunction m_HeatFunction;
    public GameObject m_BulletPrefab;
    public Image m_HitIndicator;

    private float m_SpawnTimer;

    [SyncVar]
    public bool m_Despawned;
    [SyncVar]
    public bool m_Alive;
    [SyncVar]
    public int m_Score;
    [SyncVar]
    public int m_Kills;
    [SyncVar]
    public int m_Deaths;

    private bool m_ShieldPowerUp;

    [SyncVar]
    private float m_HP;

    [SyncVar]
    private bool m_Fired;

    [SyncVar]
    private float m_ShieldHealth;

    private bool m_ReloadPowerUp;
    private float m_ReloadTimer;          // counts down to 0, tank can only shoot when not counting down
    private float m_ReloadMultiplier;     // used by powerups to speed up reloading
    private float m_ReloadPowerUpTimer;

    [SyncVar]
    private float m_Heat;

    private float m_DeathZoneDuration = 2.0f;
    private float m_DeathZoneTimer;
    private bool m_InDeathZone = false;

    private float volLowRange = .75f;
    private float volHighRange = 1.0f;
    private bool m_HitActive = false;
    private float m_IndicatorDuration = 0.0f;

    private bool m_HasFlag = false;
    FlagCaptureScript m_FlagData = null;

    Text m_FlagScoreText;

    Rigidbody rb;

    Vector3 previousPos;
    public float m_NoMovementThreshold = 0.0001f;

    public TeamColours m_PlayerTeam;
    public GameObject m_CarMat;

    public int m_PlayerNumber;

    [Command]
    public void CmdShoot()
    {
        //Shooting
        if (m_Alive)
        {
            if (m_GunData.gunType == FireType.TwinGuns)
            {
                GameObject bullet = (GameObject)Instantiate(m_BulletPrefab);  // get bullet object from pool
                if (bullet != null) // check if object pool returned a bullet
                {
                    bullet.GetComponent<OnlineBullet>().m_Owner = this;
                    if (m_ReloadTimer <= 0.0f && m_Heat < m_HeatFunction.HeatSlider.maxValue)    // only shoot if not waiting for reload
                    {
                        Debug.Log("Shooting");
                        float vol = Random.Range(volLowRange, volHighRange);

                        if (m_GunData.altguns)
                        {
                            m_GunData.Barrel1.GetComponent<AudioSource>().volume = vol;
                            m_GunData.Barrel1.GetComponent<AudioSource>().Play();
                            bullet.transform.position = m_GunData.Barrel1.transform.position;
                            bullet.transform.rotation = m_GunData.Barrel1.transform.rotation;
                            m_GunData.altguns = !m_GunData.altguns;
                        }
                        else
                        {
                            m_GunData.Barrel2.GetComponent<AudioSource>().volume = vol;
                            m_GunData.Barrel2.GetComponent<AudioSource>().Play();
                            bullet.transform.position = m_GunData.Barrel2.transform.position;
                            bullet.transform.rotation = m_GunData.Barrel2.transform.rotation;
                            m_GunData.altguns = !m_GunData.altguns;
                        }
                        bullet.SetActive(true);
                        m_ReloadTimer = m_GunData.ReloadTwinGuns;   // reset reload speed

                        // server spawns bullet on all clients (NOTE: convert this to work with the object pool, NetworkServer.Spawn instantiates)
                        NetworkServer.Spawn(bullet);
                        Destroy(bullet, 3.0f);

                        m_Heat += m_HeatFunction.BulletHeat;
                    }
                }
            }
            // solid beam from front of vehicle
            else if (m_GunData.gunType == FireType.Beam)
            {
                if (m_Heat < m_HeatFunction.HeatSlider.maxValue && !m_Fired)    // only shoot if not waiting for reload
                {
                    Debug.Log("Shooting");

                    m_Fired = true;
                }
            }

            // start windup
            else if(m_GunData.gunType == FireType.Ram)
            {
                if (m_Heat < m_HeatFunction.HeatSlider.maxValue && !m_Fired)    // only shoot if not waiting for reload
                {
                    m_Fired = true;
                }
                //GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().Move(0, 100.0f, 0.0f, 0.0f);
            }

            else if (m_GunData.gunType == FireType.Cannon)
            {
                if (m_Heat < m_HeatFunction.HeatSlider.maxValue)
                {
                    m_GunData.fired = true;
                }
            }
        }
    }

    [Command]
    public void CmdShootRelease()
    {
        if(m_GunData.gunType == FireType.Beam && m_Fired)
        {
            if(m_Heat < (m_HeatFunction.HeatSlider.maxValue / 4) * 3)
                m_Fired = false;
            m_GunData.BeamBarrel.GetComponent<AudioSource>().Stop();

            m_GunData.BeamBarrel.SetActive(false);
        }

        // shoot cannonballs NOT NETWORK CONVERTED
        if (m_GunData.gunType == FireType.Cannon && m_GunData.fired)
        {
            Debug.Log("Shooting");

            GameObject bullet1 = m_GunData.BulletPool.GetPooledObject();  // get bullet object from pool
            float vol = Random.Range(volLowRange, volHighRange);

            if (bullet1 != null) // check if object pool returned a bullet
            {
                m_GunData.Barrel1.GetComponent<AudioSource>().volume = vol;
                m_GunData.Barrel1.GetComponent<AudioSource>().Play();
                bullet1.transform.position = m_GunData.Barrel1.transform.position;
                bullet1.transform.rotation = m_GunData.Barrel1.transform.rotation;
                bullet1.GetComponent<CannonBallTravel>().m_Speed = m_Heat;
                bullet1.SetActive(true);
            }
            GameObject bullet2 = m_GunData.BulletPool.GetPooledObject();  // get bullet object from pool

            if (bullet2 != null)
            {
                m_GunData.Barrel2.GetComponent<AudioSource>().volume = vol;
                m_GunData.Barrel2.GetComponent<AudioSource>().Play();
                bullet2.transform.position = m_GunData.Barrel2.transform.position;
                bullet2.transform.rotation = m_GunData.Barrel2.transform.rotation;
                bullet2.GetComponent<CannonBallTravel>().m_Speed = m_Heat;
                bullet2.SetActive(true);
            }
            m_GunData.fired = false;
            m_Heat = 0;
        }

        // shoot forward
        if (m_GunData.gunType == FireType.Ram && m_Fired)
        {
            rb.velocity = transform.forward * m_Heat / 2;
            m_GunData.RamCollider.SetActive(true);
            m_Fired = false;
        }

        if (m_GunData.gunType == FireType.TwinGuns)
        {
            m_GunData.Barrel1.transform.GetChild(0).gameObject.SetActive(false);
            m_GunData.Barrel2.transform.GetChild(0).gameObject.SetActive(false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_Alive)
        {
            if (other.tag == "Pickup" && other.GetComponent<PickupBehaviour>().ColliderTag != "Beam")   // hack to ensure beam weapon doesn't pick up objects
            {
                other.GetComponent<AudioSource>().Play();   // play pickup noise
                PickupBehaviour pickup = other.GetComponent<PickupBehaviour>();

                switch (pickup.Type)
                {
                    case PickupType.Health:
                        if(m_HP < 100.0f)
                        {
                            if (m_HP + pickup.Value > 100.0f)
                                m_HP = 100.0f;
                            else
                                m_HP += pickup.Value;
                        }
                        Debug.Log("HP picked up!");

                        break;

                    case PickupType.Shield:
                        m_CarData.Shield.SetActive(true);
                        m_HeatFunction.HealthImage.color = Color.cyan;
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
                if (other.GetComponent<OnlineBullet>().m_Active)
                {
                    GetComponent<AudioSource>().PlayOneShot(m_CarData.HitSound, 1);
                    if (m_ShieldPowerUp)
                    {
                        m_ShieldHealth -= other.GetComponent<OnlineBullet>().m_Damage;
                    }
                    else
                    {
                        m_HP -= other.GetComponent<OnlineBullet>().m_Damage;
                        if (m_CarData.Health <= 0.0f)
                        {
                            other.GetComponent<OnlineBullet>().m_Owner.RecordKill(this);
                            Death();
                        }
                    }
                    Debug.Log("bullethit");
                    other.GetComponent<OnlineBullet>().ResetBullet();

                    RotateHitIndicator(other.transform.position);
                }
            }
            else if (other.tag == "TrainScoop")
            {
                Death();
            }
            else if (other.tag == "Hazard")
            {
                Death();
            }
            // start countdown to death
            else if (other.tag == "DeathZone")
            {
                m_InDeathZone = true;
                m_DeathZoneTimer = m_DeathZoneDuration;
                Debug.Log("In Death Zone");
            }
            else if (other.tag == "Flag")
            {
                if (m_HasFlag)
                {
                    if (other.GetComponent<FlagCaptureScript>().m_FlagColour == m_PlayerTeam)
                    {
                        m_FlagData.CaptureFlag(other.GetComponent<FlagCaptureScript>().m_FlagColour);
                        m_Score += 20;

                        int teamScore = int.Parse(m_FlagScoreText.text);
                        teamScore++;
                        m_FlagScoreText.text = teamScore.ToString();

                        m_HasFlag = false;
                        m_FlagData = null;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DeathZone")
        {
            m_InDeathZone = false;
            Debug.Log("Exiting Death Zone");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(m_Alive)
        {
            if (other.tag == "Beam")
            {
                if (m_ShieldPowerUp)
                {
                    m_ShieldHealth -= (other.GetComponent<laserScript>().m_Damage * Time.deltaTime);
                }
                else
                {
                    m_HP -= (other.GetComponent<laserScript>().m_Damage * Time.deltaTime);
                    if (m_CarData.Health <= 0.0f)
                    {
                        //other.GetComponent<BulletTravel>().m_Owner.RecordKill(this);
                        Death();
                    }
                }
            }
        }

    }

    private void RotateHitIndicator(Vector3 hitPos)
    {
        if (!m_HitActive)
        {
            m_HitActive = true;
            m_IndicatorDuration = 1.0f;
        }

        Vector3 relpos = hitPos - transform.position;

        Quaternion qRot = Quaternion.LookRotation(relpos);

        float rot = Quaternion.Angle(qRot, transform.localRotation);

        if (Vector3.Dot(transform.right, relpos) > 0.0f)
        {
            rot = -rot;
        }

        m_HitIndicator.rectTransform.localRotation = Quaternion.Euler(0, 0, rot);
    }

    public void FlagTaken(FlagCaptureScript flag)
    {
        m_FlagData = flag;
        m_HasFlag = true;
    }

    public void RecordKill(OnlineFireControl car)
    {
        if (car.m_PlayerTeam == m_PlayerTeam)
        {
            // teamkill!
            m_Kills--;
            m_Score -= 10;
        }
        else
        {
            m_Kills++;
            m_Score += 10;
        }
    }

    // Use this for initialization
    void Start () {
        m_ReloadTimer = 0.0f;
        m_GunData.BulletPool = GetComponent<CarPooler>();
        m_HeatFunction.HealthImage.color = Color.green;
        m_Alive = true;
        m_Despawned = false;
        m_Score = 0;
        m_Kills = 0;
        m_Deaths = 0;
        m_HP = m_CarData.Health;
        rb = GetComponent<Rigidbody>();
        previousPos = Vector3.zero;

        Renderer ren = m_CarMat.GetComponentInChildren<Renderer>();
        Material mat;
        if (ren.materials.Length == 1)
            mat = ren.material;
        else
            mat = ren.materials[1];

        switch (m_PlayerTeam)
        {
            case TeamColours.Red:
                mat.color = Color.red;
                m_FlagScoreText = GameObject.FindGameObjectWithTag("RedScore").GetComponent<Text>();
                break;
            case TeamColours.Blue:
                mat.color = Color.blue;
                m_FlagScoreText = GameObject.FindGameObjectWithTag("BlueScore").GetComponent<Text>();
                break;
            case TeamColours.Green:
                mat.color = Color.green;
                break;
            case TeamColours.Yellow:
                mat.color = Color.yellow;
                break;
            default:
                break;
        }

        if (m_GunData.gunType == FireType.Ram)
            m_GunData.RamCollider.SetActive(false);
    }

    private void Death()
    {

        if (m_HasFlag)
        {
            m_FlagData.DropFlag();
            m_FlagData = null;
            m_HasFlag = false;
        }

        m_Deaths++;
        m_Alive = false;
        m_InDeathZone = false;
        m_ShieldHealth = 0.0f;
        m_HP = 0.0f;
        m_Heat = 0.0f;
        m_ReloadPowerUpTimer = 0.0f;
        m_SpawnTimer = m_CarData.RespawnDuration;
        m_CarData.DeathParticles.SetActive(true);
    }

    private void Respawn()
    {
        m_Despawned = true;
        m_Alive = true;
        m_HP = 100.0f;
        m_CarData.DeathParticles.SetActive(false);
        //Transform respawnTarget = FindFurthestTarget("Respawn").transform;
        //transform.position = respawnTarget.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if(m_Alive)
        {
            if(m_InDeathZone)
            {
                m_DeathZoneTimer -= Time.deltaTime;
                if (m_DeathZoneTimer < 0.0f)
                {
                    Death();
                }
            }

            if (m_HitActive)
            {
                m_IndicatorDuration -= Time.deltaTime;
                if (m_IndicatorDuration <= 0.0f)
                {
                    m_IndicatorDuration = 0.0f;
                    m_HitActive = false;
                }
                m_HitIndicator.color = new Color(Color.red.r, Color.red.g, Color.red.b, m_IndicatorDuration);

            }

            if (m_GunData.gunType == FireType.Ram)
            {
                if (Vector3.Distance(previousPos, transform.position) < m_NoMovementThreshold)
                {
                    m_GunData.RamCollider.SetActive(false);
                }
                previousPos = transform.position;
            }

            // went off edge
            if (transform.position.y < -5.0f || m_HP <= 0.0f)
            {
                Death();
            }

            // activate smoke
            if (m_HP < 50.0f)
            {
                m_CarData.DamageParticles.SetActive(true);
            }
            else
            {
                m_CarData.DamageParticles.SetActive(false);
            }

            // update health slider
            if (m_ShieldPowerUp)
            {
                m_HeatFunction.HealthSlider.value = m_ShieldHealth;
                if (m_ShieldHealth <= 0.0f)
                {
                    Debug.Log("Deactivate Shield!");
                    m_CarData.Shield.SetActive(false);
                    m_HeatFunction.HealthImage.color = Color.green;
                    m_ShieldPowerUp = false;
                }
            }
            else
                m_HeatFunction.HealthSlider.value = m_HP;

            // power up timer
            if (m_ReloadPowerUp)
            {
                m_ReloadPowerUpTimer -= Time.deltaTime;
                if (m_ReloadPowerUpTimer <= 0.0f)
                {
                    m_ReloadPowerUp = false;
                }
            }



            if (m_GunData.gunType == FireType.Beam && m_GunData.BeamBarrel.activeInHierarchy && m_Heat < m_HeatFunction.HeatSlider.maxValue)
            {
                m_Heat += m_HeatFunction.BeamHeat * Time.deltaTime;
                if(m_Heat > m_HeatFunction.HeatSlider.maxValue)
                {
                    CmdShootRelease();
                }
            }
            else if (m_GunData.gunType == FireType.Beam && m_Fired)
            {
                m_GunData.BeamBarrel.SetActive(true);
                if (!m_GunData.BeamBarrel.GetComponent<AudioSource>().isPlaying)
                {
                    m_GunData.BeamBarrel.GetComponent<AudioSource>().Play();
                }
            }

            if (m_GunData.gunType == FireType.Cannon && m_GunData.fired)
            {
                m_Heat += m_HeatFunction.BeamHeat * Time.deltaTime; // TODO: get separate cannonball heat float
                if (m_Heat > m_HeatFunction.HeatSlider.maxValue)
                {
                    CmdShootRelease();
                }
            }

            if (m_GunData.gunType == FireType.Ram && m_Fired)
            {
                m_Heat += m_HeatFunction.RamHeat * Time.deltaTime;
                if (m_Heat > m_HeatFunction.HeatSlider.maxValue)
                {
                    CmdShootRelease();
                }
            }

            // cooldown
            if (m_Heat > 0.0f)
            {
                if (m_ReloadPowerUp)
                    m_Heat -= m_HeatFunction.HeatDrain * Time.deltaTime * m_ReloadMultiplier;
                else
                    m_Heat -= m_HeatFunction.HeatDrain * Time.deltaTime;
            }
            else
                m_Heat = 0.0f;

            m_HeatFunction.HeatSlider.value = m_Heat;

            if(m_Heat < (m_HeatFunction.HeatSlider.maxValue / 4) * 3)
            {
                m_HeatFunction.HeatImage.color = Color.yellow;
            }
            else
            {
                m_HeatFunction.HeatImage.color = Color.red;
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
                m_ReloadTimer = 0.0f;
            }
        }
        // count down respawn
        else if(!m_Despawned)
        {
            if (m_SpawnTimer > 0.0f)
            {
                m_SpawnTimer -= Time.deltaTime;
            }
            else
            {
                m_SpawnTimer = 0.0f;
                Respawn();
            }
        }      
    }

    public GameObject FindFurthestTarget(string trgt)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(trgt);

        GameObject furthest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position + position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                furthest = go;
                distance = curDistance;
            }
        }

        return furthest;
    }
}
