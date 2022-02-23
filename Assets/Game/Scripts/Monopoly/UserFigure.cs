using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.Utils;
using Assets.Game.Scripts.Utils.Extensions;
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
    [SerializeField] public int clientPosition = 0;

    [SyncVar] public int userMoney = 0;
    [SyncVar] public bool shouldMove;
    [SyncVar] public bool playerThrowDice = false;
    [SyncVar] public int prisonRemained = 0;

    public int steps = 0;
    public bool isMoving;
    public bool moveEnded;
    public bool frezeFigure;
    private bool inited;
    public int lastThrowedDiceNumber;


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
            lastThrowedDiceNumber = Game.rolledNumber;
            steps = lastThrowedDiceNumber;
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

    public void PayToUser(UserFigure figure, int amount)
    {
        this.CmdSetCurrentPlayerInd(userMoney - amount);
        this.CmdSetCurrentPlayerInd(figure.userMoney + amount);
    }

    private void OnTurnEnded()
    {
        UIHandler.buyUnitButton.gameObject.SetActive(false);
        UIHandler.payRentaButton.gameObject.SetActive(false);
        UIHandler.FieldInfoPanel.SetActive(true);

        GetCurrentUnit().OnPlayerStop(this);
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

    [Command]
    public void CmdSetPlayerPrisonRemained(int remained)
    {
        prisonRemained = remained;
        RpcSetPlayerPrisonRemained(remained);
    }

    [ClientRpc]
    public void RpcSetPlayerPrisonRemained(int remained)
    {
        prisonRemained = remained;
    }

    #region Current turn player handle
    [Command]
    public void CmdSetCurrentPlayerInd(int ind)
    {
        Debug.Log("To next user by command" + ind);

        if(ind == 0)
        {
            HandleLapPassed();
        }

        Room.CurrentUserInd = ind;
        RpcSetCurrentPlayerInd(ind);
    }

    private void HandleLapPassed()
    {
        Room.UserFigures.Where(figure => figure.prisonRemained > 0).ForEach(figure => figure.CmdSetPlayerPrisonRemained(figure.prisonRemained - 1));
    }

    [ClientRpc]
    public void RpcSetCurrentPlayerInd(int ind)
    {
        Debug.Log("To next user by rpc" + ind);
        Room.CurrentUserInd = ind;
    }

    public int GetNextPlayerIndex()
    {
        if (Room.UserFigures.All(figure => figure.prisonRemained > 0))
        {
            return GetMinPrisonRemainedUserInd();
        }

        var ind = Room.CurrentUserInd;

        do
        {
            ind++;
            ind %= Room.UserFigures.Count;
        }
        while (Room.UserFigures[ind].prisonRemained != 0);
        
        return ind;
    }

    private int GetMinPrisonRemainedUserInd()
    {
        var minPrisonRemainedUser = Room.UserFigures.FirstOrDefault(figure 
            => figure.prisonRemained == Room.UserFigures.Min(figure => figure.prisonRemained));

        Room.UserFigures.Where(figure => figure.prisonRemained > 0).ForEach(figure => figure.CmdSetPlayerPrisonRemained(figure.prisonRemained - minPrisonRemainedUser.prisonRemained));

        return Room.UserFigures.FindIndex(user => minPrisonRemainedUser);
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
