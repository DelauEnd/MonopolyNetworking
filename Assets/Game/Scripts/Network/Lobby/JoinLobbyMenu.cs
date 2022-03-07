using kcp2k;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Network.Lobby
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject joinLobbyPagePanel = null;
        [SerializeField] private GameObject landingPagePanel = null;

        [SerializeField] private TMP_InputField ipAddressInputField = null;
        [SerializeField] private TMP_InputField portInputField = null;
        [SerializeField] private Button joinButton = null;

        private void Awake()
        {
            if (networkManager == null)
                networkManager = FindObjectOfType<NetworkManagerLobby>();

            portInputField.text = networkManager.gameObject.GetComponent<KcpTransport>().Port.ToString();           
        }

        public void HideMenu()
        {
            ipAddressInputField.text = string.Empty;
            joinLobbyPagePanel.gameObject.SetActive(false);
        }

        public void JoinLobby()
        {
            networkManager.networkAddress = ipAddressInputField.text;
            networkManager.gameObject.GetComponent<KcpTransport>().Port = ushort.Parse(portInputField.text);

            networkManager.StartClient();

            joinButton.interactable = false;
        }

        private void Update()
        {
            if (!networkManager.isNetworkActive)
                joinButton.interactable = true;
        }
    }
}
