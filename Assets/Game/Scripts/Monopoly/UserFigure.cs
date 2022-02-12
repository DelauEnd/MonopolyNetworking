using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.Utils;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserFigure : NetworkBehaviour
{  
    public GameField Field = null;
    public GameManager Game = null;

    public PlayerUIHandler UIHandler = null;

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

    [SyncVar(hook = nameof(SyncCurrentPos))] 
    public int currentPosition = 0;
    private int clientPosition = 0;

    [SyncVar] public bool shouldMove;
    public int steps = 0;
    [SyncVar(hook = nameof(ChangeUserMoney))] 
    public uint userMoney = 0;
    public bool isMoving;
    public bool moveEnded;
    public bool frezeFigure;

    [SyncVar] 
    public bool playerThrowDice = false;

    //TODO: Store all player figures into the Gamefield as static List<UserFigure>. After sync update value in list via userlist[current] == this

    public override void OnStartClient()
    {
        Field = FindObjectOfType<GameField>();
        Game = FindObjectOfType<GameManager>();
        Room.UserFigures.Add(this);
        Room.PlayersCount++;

        CmdSetUserMoney(1500);
    }

    private void Update()
    {
        if (!hasAuthority)
            return;

        if (frezeFigure)
            return;

        PlayerThrowDice(false);
        if (Input.GetKeyDown(KeyCode.Space) && Room.CurrentPlayer == this)
        {
            PlayerThrowDice(true);
            Debug.Log("Try to roll dices");
            shouldMove = true;
                //steps = 1;
        }

        if (Room.CurrentPlayer == this && Game.readyToMove && shouldMove)
        {
            steps = Game.rolledNumber;
            Game.CmdSetRolledNumber(0);
            Game.CmdSetReadyToMove(false);

            moveEnded = false;
            StartCoroutine(Move());
            shouldMove = false;
            moveEnded = true;

            //var newInd = GetNextPlayerIndex();
            //Debug.Log($"New user ind {newInd}");
            //CmdCurrentPlayerToNext(newInd);
        }
    }

    IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
            clientPosition++;
            clientPosition %= GameField.fieldUnits.Count;

            if (clientPosition == 0)
                LoopPased();

            var nextPos = Field.GetAvailablePointForField(clientPosition);
            while (ShouldMoveToNext(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            steps--;
        }

        CmdChangeCurrentPos(clientPosition);
        MoveOver();
        isMoving = false;

        UIHandler.BuyUnitPanel.SetActive(true);
        frezeFigure = true;
    }

    [Command]
    public void CmdSetUserMoney(uint money)
    {
        userMoney = money;       
        RpcSetUserMoney(money);
    }

    private void ChangeUserMoney(uint oldValue, uint newValue)
    {
        UIHandler.DrawUserMoney(newValue); 
    }

    [ClientRpc]
    public void RpcSetUserMoney(uint money)
    {
        userMoney = money;
    }

    #region Current turn player handle
    [Command]
    public void CmdCurrentPlayerToNext(int ind)
    {
        Debug.Log("To next user by command" + ind);
        Room.CurrentUserInd = ind;
        RpcCurrentPlayerToNext(ind);
    }

    [ClientRpc]
    public void RpcCurrentPlayerToNext(int ind)
    {
        Debug.Log("To next user by rpc" + ind);
        Room.CurrentUserInd = ind;
    }

    public int GetNextPlayerIndex()
    {
        var ind = Room.CurrentUserInd;
        if (ind + 1 == Room.UserFigures.Count)
            return ind - 1;
        return ind + 1;
    } 
    #endregion

    #region Current position handle
    /// <summary>
    /// Method that sends command from client to server to set new value for currentPosition
    /// </summary>
    /// <remarks>
    /// this method used to sync currentPosition value between all users after player over his turn
    /// </remarks>
    /// <param name="newPos">new currentPosition value</param>
    [Command]
    private void CmdChangeCurrentPos(int newPos)
    {
        Debug.Log("CurrentPos changed by Command to server");
        this.currentPosition = newPos;
    }

    /// <summary>
    /// (SyncVar Hook)
    /// Called on server after changing currentPosition value on any player
    /// </summary>
    /// <param name="oldValue">currenetPosition value before change</param>
    /// <param name="newValue">currenetPosition value after change</param>
    void SyncCurrentPos(int oldValue, int newValue)
    {
        Debug.Log("Sync currentPos");
        RpcChangeCurrentPos(newValue);
    }

    /// <summary>
    /// Sends rpc to clients to set new value for currentPosition
    /// </summary>
    /// <param name="newPos"></param>
    [ClientRpc]
    private void RpcChangeCurrentPos(int newPos)
    {
        Debug.Log("CurrentPos changed by Rpc from server");
        this.currentPosition = newPos;
    } 
    #endregion

    [Command]
    public void PlayerThrowDice(bool throwed)
    {
        playerThrowDice = throwed;
    }

    private void MoveOver()
    {
        Debug.Log("final position:" + currentPosition);
    }

    private void LoopPased()
    {
        userMoney += 200;
        Debug.Log("Loop pased, user money:" + userMoney);
    }

    bool ShouldMoveToNext(Vector3 goal)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, Time.fixedDeltaTime * 5);

        return goal != transform.position;
    }
}
