using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using XInputDotNetPure;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class OnlineUserControl : NetworkBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private OnlineFireControl m_FireControl;

        float h;
        float v;
        float triggerFire = 0.0f;
        float cameraMovement = 0.0f;
        float brake = 0.0f;


        bool fire = false;
        bool fireRelease = false;
        public GameObject m_Camera;
        GamePadState m_GamePad;
        OnlineControllerInput m_SpawnSelector;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_FireControl = GetComponent<OnlineFireControl>();
            m_SpawnSelector = GameObject.Find("ControllerMouse").GetComponent<OnlineControllerInput>();
        }


        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            m_GamePad = GamePad.GetState(PlayerIndex.One);
            if (m_GamePad.IsConnected)
            {
                if (m_SpawnSelector.gameObject.activeInHierarchy )
                {
                    if (m_GamePad.Buttons.B == ButtonState.Pressed)
                        m_SpawnSelector.CycleSelection();

                    if (m_GamePad.Buttons.A == ButtonState.Pressed)
                        m_SpawnSelector.SelectionPressed();
                }
                if(m_FireControl.m_Alive)
                {
                    float oldTrigger = triggerFire;

                    h = m_GamePad.ThumbSticks.Left.X;
                    //v = gamePad.Buttons.A;
                    triggerFire = m_GamePad.Triggers.Right;
                    cameraMovement = m_GamePad.ThumbSticks.Right.X;

                    if (m_GamePad.Buttons.X == ButtonState.Pressed)
                        brake = 1f;
                    else
                        brake = 0f;

                    if (m_GamePad.Buttons.A == ButtonState.Pressed)
                        v = 1f;
                    else if (m_GamePad.Buttons.B == ButtonState.Pressed)
                        v = -1f;
                    else
                        v = 0f;
                    Debug.Log(v);

                    if (triggerFire > 0.5f)
                    {
                        fire = true;
                    }
                    else
                    {
                        fire = false;
                        if (triggerFire < oldTrigger && triggerFire <= 0.1f)
                        {
                            fireRelease = true;
                        }
                        else
                        {
                            fireRelease = false;
                        }
                    }
                }          
            }
        }

        private void FixedUpdate()
        {
            // pass the input to the car!
            if (!isLocalPlayer)
            {
                return;
            }

            if (!m_GamePad.IsConnected)
            {
                if (m_SpawnSelector.gameObject.activeInHierarchy)
                {
                    if (Input.GetButtonDown("Fire1"))
                        m_SpawnSelector.CycleSelection();

                    if (Input.GetButtonDown("Submit"))
                        m_SpawnSelector.SelectionPressed();
                }
                if(m_FireControl.m_Alive)
                {
                    bool shotPastFrame = fire;
                    h = CrossPlatformInputManager.GetAxis("Horizontal");
                    v = CrossPlatformInputManager.GetAxis("Vertical");
                    cameraMovement = CrossPlatformInputManager.GetAxis("AuxButtons");
                    bool braking = CrossPlatformInputManager.GetButtonDown("Jump");
                    if (braking)
                        brake = 1f;
                    else
                        brake = 0f;
                    fire = Input.GetButton("Fire1");
                    if (!fire && shotPastFrame)
                        fireRelease = true;
                    else
                        fireRelease = false;
                }            
            }

            if (fire)
                m_FireControl.CmdShoot();

            if (fireRelease)
                m_FireControl.CmdShootRelease();
            m_Camera.transform.Translate(Vector3.right * (cameraMovement * 10.0f) * Time.deltaTime);

#if !MOBILE_INPUT
            m_Car.Move(h, v, v, brake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
