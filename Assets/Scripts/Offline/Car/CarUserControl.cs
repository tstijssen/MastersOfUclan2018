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

        bool fire = false;
        bool fireRelease = false;

        string fireCommand;
        string horizontalCommand;
        string verticalCommand;

        public GamePadState gamePad;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_FireControl = GetComponent<CarFireControl>();
            fireCommand = GetComponentInParent<LocalPlayerSetup>().m_FireCommand;
            horizontalCommand = GetComponentInParent<LocalPlayerSetup>().m_HorizontalMove;
            verticalCommand = GetComponentInParent<LocalPlayerSetup>().m_VerticalMove;
            gamePad = GetComponentInParent<LocalPlayerSetup>().m_GamePadState;
        }

        private void Update()
        {
            if (!m_FireControl.m_Alive)
            {
                return;
            }
            Debug.Log("Player is using gamepad " + gamePad.IsConnected);

            if (gamePad.IsConnected)
            {
                gamePad = GetComponentInParent<LocalPlayerSetup>().m_GamePadState;

                h = gamePad.ThumbSticks.Left.X;
                v = gamePad.ThumbSticks.Left.Y;

                fire = (gamePad.Buttons.A == ButtonState.Pressed);

                fireRelease = (gamePad.Buttons.A == ButtonState.Released);
  
                Debug.Log(fire);
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

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
