using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Network.Lobby
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        private void Awake()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }
        private void HandleClientDisconnected()
        {
            gameObject.SetActive(true);
            landingPagePanel.SetActive(true);
        }

        private void HandleClientConnected()
        {
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}
