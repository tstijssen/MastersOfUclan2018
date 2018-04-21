using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using XInputDotNetPure;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private CarFireControl m_FireControl;

        float h;
        float v;
        float triggerFire = 0.0f;
        float cameraMovement = 0.0f;
        
        bool fire = false;
        bool fireRelease = false;

        string fireCommand;
        string horizontalCommand;
        string verticalCommand;

        public GamePadState gamePad;
        GameObject m_Camera;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_FireControl = GetComponent<CarFireControl>();
            fireCommand = GetComponentInParent<LocalPlayerSetup>().m_FireCommand;
            horizontalCommand = GetComponentInParent<LocalPlayerSetup>().m_HorizontalMove;
            verticalCommand = GetComponentInParent<LocalPlayerSetup>().m_VerticalMove;
            gamePad = GetComponentInParent<LocalPlayerSetup>().m_GamePadState;
            m_Camera = GetComponentInParent<LocalPlayerSetup>().m_Camera;
        }

        private void Update()
        {
            
            if (!m_FireControl.m_Alive)
            {
                return;
            }

            if (gamePad.IsConnected)
            {
                float oldTrigger = triggerFire;
                gamePad = GetComponentInParent<LocalPlayerSetup>().m_GamePadState;

                h = gamePad.ThumbSticks.Left.X;
                v = gamePad.ThumbSticks.Left.Y;
                triggerFire = gamePad.Triggers.Right;
                cameraMovement = gamePad.ThumbSticks.Right.X;

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

                //fire = (gamePad.Buttons.A == ButtonState.Pressed);

                //fireRelease = (gamePad.Buttons.A == ButtonState.Released);
            }
        }

        private void FixedUpdate()
        {

            if (!m_FireControl.m_Alive)
            {
                return;
            }


            if (!gamePad.IsConnected)
            {
                h = CrossPlatformInputManager.GetAxis(horizontalCommand);
                v = CrossPlatformInputManager.GetAxis(verticalCommand);

                fire = CrossPlatformInputManager.GetButton(fireCommand);
                fireRelease = CrossPlatformInputManager.GetButtonUp(fireCommand);
            }

            // pass the input to the car!

            if (fire)
                m_FireControl.Shoot();
            if (fireRelease)
                m_FireControl.ShootRelease();

            m_Camera.transform.Translate(Vector3.right * (cameraMovement * 10.0f) * Time.deltaTime);
           // Debug.Log(v);
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
