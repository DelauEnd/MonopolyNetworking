using Assets.Game.Scripts.Utils.Extensions;
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
    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private TMP_Text[] playersNameTexts = new TMP_Text[5];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[5];
        [SerializeField] private RawImage[] playerColorImages = new RawImage[5];
        [SerializeField] private Button startGameButton = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;
        [SyncVar(hook = nameof(HandleUserColorChanged))]
        public Color DisplayColor = Color.white;

        private bool isLeader = false;
        public bool IsLeader
        {
            set
            {
                isLeader = value;
                startGameButton.gameObject.SetActive(value);
            }
        }

        private NetworkManagerLobby room;
        public NetworkManagerLobby Room
        {
            get
            {
                if (room != null)
                    return room;
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerNameInput.DisplayName);
            CmdSetDisplayColor(PlayerNameInput.DisplayColor);

            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public override void OnStopAuthority()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
        public void HandleUserColorChanged(Color oldValue, Color newValue) => UpdateDisplay();


        private void UpdateDisplay()
        {
            if(!hasAuthority)
            {
                Room.RoomPlayers.Where(player => player.hasAuthority).ForEach(player => player.UpdateDisplay());
                return;
            }

            for (int i = 0; i < playersNameTexts.Length; i++)
            {
                playersNameTexts[i].text = "Waiting for player...";
                playerReadyTexts[i].text = string.Empty;
                playerColorImages[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playersNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>READY</color>" :
                    "<color=red>NOT READY</color>";
                playerColorImages[i].gameObject.SetActive(true);
                playerColorImages[i].color = Room.RoomPlayers[i].DisplayColor;
            }
        }

        public void ExitLobby()
        {
            if (isLeader)
                Room.StopHost();       
            else
                Room.StopClient();
        }

        internal void HandleReadyToStart(bool ready)
        {
            if (!isLeader)
                return;

            startGameButton.interactable = ready;
        }

        [Command]
        private void CmdSetDisplayName(string name)
        {
            DisplayName = name;
        }

        [Command]
        private void CmdSetDisplayColor(Color color)
        {
            DisplayColor = color;
        }

        [Command]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;

            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
                return;

            Room.StartGame();
        }
    }
}
