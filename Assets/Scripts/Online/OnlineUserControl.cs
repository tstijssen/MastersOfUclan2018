using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class OnlineUserControl : NetworkBehaviour
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
            m_FireControl = GetComponentInChildren<CarFireControl>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            if (!isLocalPlayer)
            {
                return;
            }

            if(!m_FireControl)
                m_FireControl = GetComponentInChildren<CarFireControl>();

            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");
            fire = CrossPlatformInputManager.GetButton("Fire1");
            fireRelease = CrossPlatformInputManager.GetButtonUp("Fire1");

            if (fire)
                CmdShoot();
            if (fireRelease)
                CmdShootRelease();
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
        [Command]
        void CmdShoot()
        {
            NetworkServer.Spawn(m_FireControl.Shoot());
        }

        void CmdShootRelease()
        {
            m_FireControl.ShootRelease();
        }
    }
}
