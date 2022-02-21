using Assets.Game.Scripts.Monopoly.FieldUnits;
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
    [Header("Game additions")]
    public GameField Field = null;
    public GameManager Game = null;

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

    [Header("User handlers")]
    public PlayerUIHandler UIHandler = null;
    public UIController UIController = null;
    public Outline UserOutline = null;

    [Header("User info")]
    [SyncVar(hook = nameof(SyncCurrentPos))] public int currentPosition = 0;
    [SerializeField] private int clientPosition = 0;

    [SyncVar] public int userMoney = 0;
    [SyncVar] public bool shouldMove;
    [SyncVar] public bool playerThrowDice = false;

    public int steps = 0;
    public bool isMoving;
    public bool moveEnded;
    public bool frezeFigure;
    private bool inited;

    //TODO: Add color select, user figure will outlined with selected color

    public override void OnStartClient()
    {
        Field = FindObjectOfType<GameField>();
        Game = FindObjectOfType<GameManager>();

        Room.UserFigures.Add(this);
        Room.PlayersCount++;
    }

    public override void OnStartAuthority()
    {
        UIController.LockCursor();
    }

    private void Update()
    {
        if (!hasAuthority)
            return;

        InitPlayer();

        if (frezeFigure)
            return;

        CmdPlayerThrowDice(false);
        if (Input.GetKeyDown(KeyCode.Space) && Room.CurrentPlayer == this)
        {
            CmdPlayerThrowDice(true);
            Debug.Log("Try to roll dices");
            shouldMove = true;
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
        }
    }

    private void InitPlayer()
    {
        if (inited)
            return;
        inited = true;

        CmdSetUserMoney(1500);
        SetFigureOutline();
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
            clientPosition %= Field.fieldUnits.Count;

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

        OnTurnEnded();
        UIController.UnlockCursor();
        frezeFigure = true;
    }

    private void OnTurnEnded()
    {
        UIHandler.buyUnitButton.gameObject.SetActive(false);
        UIHandler.payRentaButton.gameObject.SetActive(false);
        UIHandler.FieldInfoPanel.SetActive(true);

        GetCurrentUnit().OnPlayerStop(this);

        //if (GetCurrentUnit().IsBuyable())
        //    UIHandler.buyUnitButton.gameObject.SetActive(true);
        //else if(GetCurrentUnit().IsImproveableForUser(this))
        //    throw new NotImplementedException(); //User owns unit and can improve it
        //else if(GetCurrentUnit().IsUserShouldPayRenta(this))
        //    throw new NotImplementedException(); //Unit alredy in ownership by other player

    }   

    public IFieldUnit GetCurrentUnit()
        => Field.fieldUnits[clientPosition];

    [Command(requiresAuthority = false)]
    public void CmdSetUserMoney(int money)
    {
        userMoney = money;       
        RpcSetUserMoney(money);
    }

    [ClientRpc]
    public void RpcSetUserMoney(int money)
    {
        userMoney = money;

        UIHandler.DrawUserMoney(money);
        Debug.Log($"Draw money changed to {money}");
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
    public void CmdPlayerThrowDice(bool throwed)
    {
        playerThrowDice = throwed;
    }

    private void MoveOver()
    {
        Debug.Log("final position:" + currentPosition);
    }

    private void LoopPased()
    {
        CmdSetUserMoney(userMoney+200);
        Debug.Log("Loop pased, user money:" + userMoney);
    }

    bool ShouldMoveToNext(Vector3 goal)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, Time.fixedDeltaTime * 5);

        return goal != transform.position;
    }

    /// <summary>
    /// Returns gameplayer object assigned to current figure
    /// </summary>
    /// <remarks>
    /// Dont use on client Rpc
    /// </remarks>
    /// <returns></returns>
    public NetworkGamePlayerLobby GetUserInfo()
        => Room.GamePlayers.FirstOrDefault(x => x.connectionToClient == this.connectionToClient);

    #region Outline handle
    private void SetFigureOutline()
    {
        var color = GetUserInfo().DisplayColor;

        CmdSetFigureOutline(color);
    }

    [Command]
    private void CmdSetFigureOutline(Color color)
    {
        Debug.Log("setted outline by cmd");
        SetOutline(color);
        RpcSetFigureOutline(color);
    }

    [ClientRpc]
    private void RpcSetFigureOutline(Color color)
    {
        Debug.Log("setted outline by rpc");
        SetOutline(color);
    }

    private void SetOutline(Color color)
    {
        UserOutline.OutlineColor = color;
    } 
    #endregion
}
