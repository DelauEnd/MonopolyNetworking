using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.Utils.Extensions;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
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

    public DiceCheck dices;

    [SyncVar] public int rolledNumber;
    [SyncVar] public bool readyToMove;

    //unused
    [ClientRpc]
    public void RpcSetReadyToMove(bool ready)
    {
        Debug.Log($"Ready to move setted to {ready}");
        readyToMove = ready;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetReadyToMove(bool ready)
    {
        readyToMove = ready;
        RpcSetReadyToMove(ready);
    }

    //unused
    [ClientRpc]
    public void RpcSetRolledNumber(int number)
    {
        Debug.Log($"Rolled number setted to {number}");
        rolledNumber = number;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetRolledNumber(int number)
    {
        rolledNumber = number;
        RpcSetRolledNumber(number);
    }

    private void Start()
    {
        dices = FindObjectOfType<DiceCheck>();
    }

    private void Update()
    {     
        if (Room.CurrentPlayer.playerThrowDice && !dices.dicesRolled /*&& !currentUser.Value.isMoving && !previousUser.Value.isMoving*/)
        {
            Debug.Log($"Dices rolled by user on field");
            dices.ClearRolledNumbers();
            dices.RollAllDices();
        }

        if (dices.IsNumbersCalculated && dices.dicesRolled)
        {
            //change to use server commands except of direct changes            
            RpcSetDicesRolled(false);
            rolledNumber = dices.RolledSum;
            readyToMove = true;
        }
    }

    [ClientRpc]
    public void RpcSetDicesRolled(bool rolled)
    {
        dices.dicesRolled = rolled;
    }

    //[Server]
    //public void NextUser()
    //{
    //    Debug.Log("To next user by server");
    //    ToNextUser();
    //    //RpcNextUser();
    //}

    //[ClientRpc]
    //public void RpcNextUser()
    //{
    //    Debug.Log("To next user by rpc");
    //    ToNextUser();
    //}
}
