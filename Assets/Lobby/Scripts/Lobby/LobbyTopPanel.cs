using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

namespace Prototype.NetworkLobby
{
    public class LobbyTopPanel : MonoBehaviour
    {
        public bool isInGame = false;
        public GameObject lobbyCamera;
        protected bool isDisplayed = true;
        protected Image panelImage;
        bool canInteract = true;
        bool lobbyCamActive = true;

        void Start()
        {
            panelImage = GetComponent<Image>();
        }


        void Update()
        {
            lobbyCamera.SetActive(!isInGame);

            if (!isInGame)
            {
                if(!lobbyCamActive)
                {
                    this.transform.parent.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                    this.transform.parent.GetComponent<Canvas>().worldCamera = lobbyCamera.GetComponent<Camera>();
                    lobbyCamActive = true;
                }

                return;
            }
            else
            {
                if(lobbyCamActive)
                {
                    this.transform.parent.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    lobbyCamActive = false;
                }
            }

            GamePadState m_GamePad = GamePad.GetState(PlayerIndex.One);

            if (Input.GetKeyDown(KeyCode.Escape) || (canInteract && m_GamePad.IsConnected && m_GamePad.Buttons.Start == ButtonState.Pressed))
            {
                ToggleVisibility(!isDisplayed);
            }

        }

        public void ToggleVisibility(bool visible)
        {
            isDisplayed = visible;

            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(isDisplayed);
            }

            if (panelImage != null)
            {
                panelImage.enabled = isDisplayed;
            }
            canInteract = false;
            StartCoroutine(ButtonClick());
        }

        IEnumerator ButtonClick()
        {
            yield return new WaitForSeconds(0.25f);
            canInteract = true;   // After the wait is over, the player can interact with the menu again.
        }
    }
}