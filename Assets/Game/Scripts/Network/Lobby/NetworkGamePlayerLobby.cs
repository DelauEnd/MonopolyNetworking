using Assets.Game.Scripts.Utils.Extensions;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Network.Lobby
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        [SerializeField]
        public string DisplayName = "Loading...";

        [SyncVar]
        [SerializeField]
        public Color DisplayColor = Color.white;

        [SyncVar]
        [SerializeField]
        public int UserFigure = 0;

        public UserFigure userFigure = null;

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

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);
        }
        public override void OnStopClient()
        {
        }

        public override void OnStartAuthority()
        {
            NetworkManagerLobby.OnClientDisconnected += DisconnectUser;
        }

        public override void OnStopAuthority()
        {
            NetworkManagerLobby.OnClientDisconnected -= DisconnectUser;
        }

        private void DisconnectUser()
        {
            SceneManager.LoadScene("Scene_MainMenu");
            Cursor.lockState = CursorLockMode.None;
        }

        

        /// <summary>
        /// Returns user figure object assigned to current player
        /// </summary>
        /// <remarks>
        /// Dont use on client Rpc
        /// </remarks>
        /// <returns></returns>
        [Obsolete]
        public UserFigure GetUserFigure()
            => Room.UserFigures.FirstOrDefault(x => x.connectionToClient == this.connectionToClient);

        [Server]
        public void SetDisplayName(string name)
        {
            this.DisplayName = name;
        }

        [Server]
        public void SetDisplayColor(Color color)
        {
            this.DisplayColor = color;
        }
    }
}
