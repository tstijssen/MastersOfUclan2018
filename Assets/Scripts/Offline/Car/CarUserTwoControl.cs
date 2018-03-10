using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserTwoControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private CarFireControl m_FireControl;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_FireControl = GetComponent<CarFireControl>();
        }


        private void FixedUpdate()
        {
            
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal2");
            float v = CrossPlatformInputManager.GetAxis("Vertical2");

            bool fire = CrossPlatformInputManager.GetButton("FireTwo");
            bool fireRelease = CrossPlatformInputManager.GetButtonUp("FireTwo");

            if(fire)
                m_FireControl.Shoot();

            if (fireRelease)
                m_FireControl.ShootRelease();

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump2");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
