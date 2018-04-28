using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

[System.Serializable]
public enum FireType {TwinGuns, Beam, Ram, Cannon };

[System.Serializable]
public enum TeamColours { Red, Blue, Green, Yellow };

[System.Serializable]
public struct HeatFunction
{
    public Slider HealthSlider;
    public Slider HeatSlider;
    public Image HealthImage;
    public Image HeatImage;

    public float BeamHeat;
    public float BulletHeat;
    public float RamHeat;
    public float HeatDrain;
}

[System.Serializable]
public struct GunData
{
    public FireType gunType;

    public GameObject Barrel1;        // location for spawning shot projectiles
    public GameObject Barrel2;        // location for spawning shot projectiles
    public CarPooler BulletPool;

    public GameObject BeamBarrel;
    public GameObject RamCollider;
    public bool altguns;
    public bool fired;
    public float ReloadTwinGuns;
}

[System.Serializable]
public struct CarData
{
    public float Health;
    public float RespawnDuration;
    public GameObject DamageParticles;
    public GameObject DeathParticles;
    public AudioClip HitSound;

    public Text[] ScoreText;

    public GameObject Shield;
}

public class CarFireControl : MonoBehaviour {

    public CarData m_CarData;
    public GunData m_GunData;
    public HeatFunction m_HeatFunction;

    public Image m_HitIndicator;

    private float m_SpawnTimer;
    public bool m_Despawned;
    public bool m_Alive;
    public int m_Score;
    public int m_Kills;
    public int m_Deaths;

    private bool m_ShieldPowerUp;
    private float m_ShieldHealth;

    private bool m_ReloadPowerUp;
    private float m_ReloadTimer;          // counts down to 0, tank can only shoot when not counting down
    private float m_ReloadMultiplier;     // used by powerups to speed up reloading
    private float m_ReloadPowerUpTimer;

    private float m_Heat;

    private float m_DeathZoneDuration = 2.0f;
    private float m_DeathZoneTimer;
    private bool m_InDeathZone = false;

    private float volLowRange = .75f;
    private float volHighRange = 1.0f;
    private bool m_HitActive = false;
    private float m_IndicatorDuration = 0.0f;

    private bool m_HasFlag = false;
    LocalFlagScript m_FlagData = null;
    Text m_FlagScoreText;

    Rigidbody rb;
    Vector3 previousPos;
    public float m_NoMovementThreshold = 0.0001f;

    public TeamColours m_PlayerTeam;
    public GameObject[] m_CarMat;
    public int m_PlayerNumber;
    public bool m_Victory;  // determines whether this player has achieved victory
    HatCaptureScript m_HatCapture = null;
    public Transform m_HatPosition;
    bool m_HasHat = false;
    float m_HatTimer = 0.0f;

    float m_RumbleCountDown = 0.0f;
    bool m_RumbleActive;

    public GameObject Shoot()
    {
        //Shooting
        if (m_Alive)
        {
            if (m_GunData.gunType == FireType.TwinGuns)
            {
                GameObject bullet = m_GunData.BulletPool.GetPooledObject();  // get bullet object from pool             
                if (bullet != null) // check if object pool returned a bullet
                {
                    bullet.GetComponent<BulletTravel>().m_Owner = this;
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
                            m_GunData.Barrel1.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        {
                            m_GunData.Barrel2.GetComponent<AudioSource>().volume = vol;
                            m_GunData.Barrel2.GetComponent<AudioSource>().Play();
                            bullet.transform.position = m_GunData.Barrel2.transform.position;
                            bullet.transform.rotation = m_GunData.Barrel2.transform.rotation;
                            m_GunData.altguns = !m_GunData.altguns;
                            m_GunData.Barrel2.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        bullet.SetActive(true);
                        m_ReloadTimer = m_GunData.ReloadTwinGuns;   // reset reload speed

                        m_Heat += m_HeatFunction.BulletHeat;
                        return bullet;
                    }
                }
            }
            // solid beam from front of vehicle
            if (m_GunData.gunType == FireType.Beam)
            {
                if (m_Heat < m_HeatFunction.HeatSlider.maxValue && !m_GunData.fired)    // only shoot if not waiting for reload
                {
                    Debug.Log("Shooting");
                    m_GunData.BeamBarrel.SetActive(true);
                    if(!m_GunData.BeamBarrel.GetComponent<AudioSource>().isPlaying)
                    {
                        m_GunData.BeamBarrel.GetComponent<AudioSource>().Play();
                    }
                    m_GunData.fired = true;
                }
            }

            // start windup
            if(m_GunData.gunType == FireType.Ram)
            {
                if (m_Heat < m_HeatFunction.HeatSlider.maxValue && !m_GunData.fired)    // only shoot if not waiting for reload
                {
                    m_GunData.fired = true;

                }
                //GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().Move(0, 100.0f, 0.0f, 0.0f);
            }

            if(m_GunData.gunType == FireType.Cannon)
            {
                if(m_Heat < m_HeatFunction.HeatSlider.maxValue)
                {
                    m_GunData.fired = true;
                }
            }
        }
        return null;
    }

    public void ShootRelease()
    {
        // stop shooting lazerbeam
        if(m_GunData.gunType == FireType.Beam && m_GunData.fired)
        {
            if(m_Heat < (m_HeatFunction.HeatSlider.maxValue / 4) * 3)
                m_GunData.fired = false;
            m_GunData.BeamBarrel.GetComponent<AudioSource>().Stop();

            m_GunData.BeamBarrel.SetActive(false);
        }

        // shoot cannonballs
        if(m_GunData.gunType == FireType.Cannon && m_GunData.fired)
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
                bullet1.GetComponent<CannonBallTravel>().m_Owner = this;
                bullet1.GetComponent<CannonBallTravel>().direction = bullet1.transform.forward;
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
                bullet2.GetComponent<CannonBallTravel>().m_Owner = this;
                bullet2.GetComponent<CannonBallTravel>().direction = bullet2.transform.forward;
                bullet2.SetActive(true);
            }
            m_GunData.fired = false;
            m_Heat = 0;
        }

        // shoot forward
        if (m_GunData.gunType == FireType.Ram && m_GunData.fired)
        {
            rb.velocity = transform.forward * m_Heat / 2;
            m_GunData.RamCollider.SetActive(true);
            m_GunData.fired = false;
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
                        if(m_CarData.Health < 100.0f)
                        {
                            if (m_CarData.Health + pickup.Value > 100.0f)
                                m_CarData.Health = 100.0f;
                            else
                                m_CarData.Health += pickup.Value;
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
                if (other.GetComponent<BulletTravel>().m_Active)
                {
                    GetComponent<AudioSource>().PlayOneShot(m_CarData.HitSound, 1);
                    if (m_ShieldPowerUp)
                    {
                        m_ShieldHealth -= other.GetComponent<BulletTravel>().m_Damage;
                    }
                    else
                    {
                        m_CarData.Health -= other.GetComponent<BulletTravel>().m_Damage;
                        if (m_CarData.Health <= 0.0f)
                        {
                            other.GetComponent<BulletTravel>().m_Owner.RecordKill(this);
                            Death();
                        }
                    }
                    Debug.Log("bullethit");

                    other.GetComponent<BulletTravel>().ResetBullet();

                    RotateHitIndicator(other.transform.position);
                }
            }
            else if (other.tag == "Shell")
            {
                if (other.GetComponent<CannonBallTravel>().m_Active)
                {
                    GetComponent<AudioSource>().PlayOneShot(m_CarData.HitSound, 1);
                    if (m_ShieldPowerUp)
                    {
                        m_ShieldHealth -= other.GetComponent<CannonBallTravel>().m_Damage;
                    }
                    else
                    {
                        m_CarData.Health -= other.GetComponent<CannonBallTravel>().m_Damage;
                        if (m_CarData.Health <= 0.0f)
                        {
                            other.GetComponent<CannonBallTravel>().m_Owner.RecordKill(this);
                            Death();
                        }
                    }
                    Debug.Log("cannon ball hit");
                    other.GetComponent<CannonBallTravel>().ResetBullet();

                    RotateHitIndicator(other.transform.position);
                }
            }
            else if (other.tag == "TrainScoop")
            {
                RotateHitIndicator(other.transform.position);
                other.transform.parent.parent.parent.GetComponentInParent<CarFireControl>().RecordKill(this);
                
                Death();
            }
            else if (other.tag == "Hazard")
            {
                if (m_HasHat)
                {
                    m_HatCapture.ResetHat();
                    m_HasHat = false;
                    m_HatCapture = null;
                }
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
                    if (other.GetComponent<LocalFlagScript>().m_FlagColour == m_PlayerTeam)
                    {
                        m_FlagData.CaptureFlag(other.GetComponent<LocalFlagScript>().m_FlagColour);
                        m_Score += 20;

                        int teamScore = int.Parse(m_FlagScoreText.text);
                        teamScore++;
                        m_FlagScoreText.text = teamScore.ToString();

                        if (teamScore >= other.GetComponent<LocalFlagScript>().m_VictoryNumber)
                        {
                            // VICTORY
                            m_Victory = true;                   
                        }

                        m_HasFlag = false;
                        m_FlagData = null;
                    }
                }
            }
        }
    }

    public void RumblePlayer(float amount)
    {
        GamePad.SetVibration((PlayerIndex)m_PlayerNumber, amount, amount);
        m_RumbleCountDown = 0.2f; // amount in seconds for rumble to reset
        m_RumbleActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DeathZone")
        {
            m_InDeathZone = false;
            Debug.Log("Leaving Death Zone");
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
                    m_CarData.Health -= (other.GetComponent<laserScript>().m_Damage * Time.deltaTime);
                    if (m_CarData.Health <= 0.0f)
                    {
                        other.transform.parent.GetComponentInParent<CarFireControl>().RecordKill(this);
                        Death();
                    }
                }
                RotateHitIndicator(other.transform.position);
            }
        }

    }

    private void RotateHitIndicator(Vector3 hitPos)
    {
        RumblePlayer(0.5f);
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

    public void FlagTaken(LocalFlagScript flag)
    {
        m_FlagData = flag;
        m_HasFlag = true;
    }

    public void HatTaken(HatCaptureScript hat)
    {
        m_HatCapture = hat;
        hat.transform.position = m_HatPosition.position;
        hat.transform.rotation = m_HatPosition.rotation;
        m_HasHat = true;
    }

    public void RecordKill(CarFireControl car)
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

    void OnEnable()
    {
        for (int m = 0; m < m_CarMat.Length; ++m)
        {
            Renderer ren = m_CarMat[m].GetComponentInChildren<Renderer>();
            Material mat;
            if (ren.materials.Length == 1)
                mat = ren.material;
            else
                mat = ren.materials[1];

            switch (GetComponentInParent<LocalPlayerSetup>().m_PlayerTeam)
            {
                case TeamColours.Red:
                    mat.color = Color.red;
                    if (GameObject.FindGameObjectWithTag("RedScore"))
                    {
                        m_FlagScoreText = GameObject.FindGameObjectWithTag("RedScore").GetComponent<Text>();

                    }
                    break;
                case TeamColours.Blue:
                    mat.color = Color.blue;
                    if (GameObject.FindGameObjectWithTag("BlueScore"))
                    {
                        m_FlagScoreText = GameObject.FindGameObjectWithTag("BlueScore").GetComponent<Text>();
                    }
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
        rb = GetComponent<Rigidbody>();
        previousPos = Vector3.zero;

        m_Victory = false;

        if(m_GunData.gunType == FireType.Ram)
            m_GunData.RamCollider.SetActive(false);

    }

    public void Death()
    {
        RumblePlayer(0.5f);
        if (m_HasFlag)
        {
            m_FlagData.DropFlag();
            m_FlagData = null;
            m_HasFlag = false;
        }
        if(m_HasHat)
        {
            m_HatCapture.DropHat();
            m_HatCapture = null;
            m_HasHat = false;
        }

        m_Deaths++;
        m_Alive = false;
        m_InDeathZone = false;
        m_ShieldHealth = 0.0f;
        m_CarData.Health = 0.0f;
        m_Heat = 0.0f;
        m_ReloadPowerUpTimer = 0.0f;
        m_SpawnTimer = m_CarData.RespawnDuration;
        m_CarData.DeathParticles.SetActive(true);
    }

    public void Respawn()
    {
        m_Despawned = true;
        m_Alive = true;
        m_CarData.Health = 100.0f;
        m_CarData.DeathParticles.SetActive(false);
        //Transform respawnTarget = FindFurthestTarget("Respawn").transform;
        //transform.position = respawnTarget.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (m_RumbleActive)
        {
            m_RumbleCountDown -= Time.deltaTime;
            if (m_RumbleCountDown < 0.0f)
            {
                RumblePlayer(0.0f);
                m_RumbleActive = false;
            }
        }

        if(m_Alive)
        {
            // if player has the hat, increase score by 10 each second
            if(m_HasHat)
            {
                m_HatTimer -= Time.deltaTime;
                if(m_HatTimer < 0.0f)
                {
                    m_HatTimer = 1.0f;
                    m_Score += 10;
                    if(m_Score >= m_HatCapture.m_VictoryNumber)
                    {
                        m_Victory = true;
                    }
                }
            }
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
            if (transform.position.y < -5.0f || m_CarData.Health <= 0.0f)
            {
                Death();
            }

            // activate smoke
            if (m_CarData.Health < 50.0f)
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
                m_HeatFunction.HealthSlider.value = m_CarData.Health;

            // power up timer
            if (m_ReloadPowerUp)
            {
                m_ReloadPowerUpTimer -= Time.deltaTime;
                if (m_ReloadPowerUpTimer <= 0.0f)
                {
                    m_ReloadPowerUp = false;
                }
            }

            if(m_GunData.gunType == FireType.Beam && m_GunData.BeamBarrel.activeInHierarchy && m_Heat < m_HeatFunction.HeatSlider.maxValue)
            {
                m_Heat += m_HeatFunction.BeamHeat * Time.deltaTime;
                if(m_Heat > m_HeatFunction.HeatSlider.maxValue)
                {
                    ShootRelease();
                }
            }

            if (m_GunData.gunType == FireType.Cannon && m_GunData.fired)
            {
                m_Heat += m_HeatFunction.BeamHeat * Time.deltaTime; // TODO: get separate cannonball heat float
                if (m_Heat > m_HeatFunction.HeatSlider.maxValue)
                {
                    ShootRelease();
                }
            }

            if(m_GunData.gunType == FireType.Ram && m_GunData.fired)
            {
                m_Heat += m_HeatFunction.RamHeat * Time.deltaTime;
                if (m_Heat > m_HeatFunction.HeatSlider.maxValue)
                {
                    ShootRelease();
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
