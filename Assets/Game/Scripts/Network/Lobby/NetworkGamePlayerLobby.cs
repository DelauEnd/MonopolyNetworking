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
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        [SerializeField]
        private string DisplayName = "Loading...";

        [SyncVar]
        [SerializeField]
        private Color DisplayColor = Color.white;

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
            Room.GamePlayers.Remove(this);
        }

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
