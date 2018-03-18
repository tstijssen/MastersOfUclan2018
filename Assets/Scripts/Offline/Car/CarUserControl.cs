using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_FireControl = GetComponent<CarFireControl>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!

           

            if (transform.parent.name == "Player1")
            {
              h = CrossPlatformInputManager.GetAxis("Horizontal");
              v = CrossPlatformInputManager.GetAxis("Vertical");
              fire = CrossPlatformInputManager.GetButton("Fire1");
              fireRelease = CrossPlatformInputManager.GetButtonUp("Fire1");
            }

            if (transform.parent.name == "Player2")
            {
                h = CrossPlatformInputManager.GetAxis("Horizontal2");
                v = CrossPlatformInputManager.GetAxis("Vertical2");
                fire = CrossPlatformInputManager.GetButton("FireTwo");
                fireRelease = CrossPlatformInputManager.GetButtonUp("FireTwo");
            }

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
