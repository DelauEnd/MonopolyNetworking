using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Game.Scripts.Network.Lobby
{
    public class SpawnManager : NetworkBehaviour
    {
        public static List<Transform> spawnPoints = new List<Transform>();

        [SerializeField] private GameObject playerPrefab = null;

        public List<NetworkRoomPlayerLobby> RoomPlayers { get; }
            = new List<NetworkRoomPlayerLobby>();

        public static int usedPoints = 0;

        public static void AddSpawnPoints(List<Transform> points)
        {
            spawnPoints.AddRange(points);

            spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void ClearSpawnPoint()
            => spawnPoints.Clear();

        public override void OnStartServer()
            => NetworkManagerLobby.OnServerReadied += SpawnPlayers;

        [ServerCallback]
        private void OnDestroy()
        { 
            NetworkManagerLobby.OnServerReadied -= SpawnPlayers;
            RoomPlayers.Clear();
        }

        [Server]
        private void SpawnPlayers(NetworkConnection conn)
        {
            var spawnPoint = spawnPoints.ElementAtOrDefault(usedPoints);

            if (spawnPoint == null)
            {
                Debug.LogError($"Missing spawn point: {usedPoints}");
                return;
            }

            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            NetworkServer.Spawn(playerInstance, conn);          

            usedPoints++;
        }
    }
}
