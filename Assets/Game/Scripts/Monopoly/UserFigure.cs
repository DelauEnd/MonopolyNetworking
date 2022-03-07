using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.UIHandlers.InGameUI;
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
    [SyncVar(hook = nameof(SyncUserInfo))] public NetworkGamePlayerLobby UserInfo = null;
    [SyncVar(hook = nameof(SyncCurrentPos))] public int currentPosition = 0;
    [SerializeField] public int clientPosition = 0;

    [SyncVar] public int userMoney = 0;
    [SyncVar] public bool shouldMove;
    [SyncVar] public bool playerThrowDice = false;
    [SyncVar] public int prisonRemained = 0;

    public bool isMoving;
    public bool moveEnded;
    public bool frezeFigure;
    private bool inited;
    public int lastThrowedDiceNumber;

    private bool colored = false;

    public bool shouldUpdateSelfInfo;

    public override void OnStartClient()
    {
        Field = FindObjectOfType<GameField>();
        Game = FindObjectOfType<GameManager>();

        Room.UserFigures.Add(this);
        Room.PlayersCount++;
    }

    public override void OnStopClient()
    {
        
    }

    public override void OnStartAuthority()
    {
        UIController.LockCursor();        
    }

    #region UserInfoHandle  
    public void SyncUserInfo(NetworkGamePlayerLobby oldValue, NetworkGamePlayerLobby newValue)
    {
        CmdInitUserInfo(newValue);
    }

    [Command(requiresAuthority = false)]
    private void CmdInitUserInfo(NetworkGamePlayerLobby userInfo)
    {
        UserInfo = userInfo;
        RpcInitUserInfo(userInfo);
    }

    [ClientRpc]
    private void RpcInitUserInfo(NetworkGamePlayerLobby userInfo)
    {
        UserInfo = userInfo;
    } 
    #endregion

    private void Update()
    {
        ColorFigure();

        if (!hasAuthority)
            return;

        CheckUserInfos();
        UpdateSelfInfo();
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
            GameManager.LastRolledNumber = Game.rolledNumber;
            Game.CmdSetRolledNumber(0);
            Game.CmdSetReadyToMove(false);

            moveEnded = false;
            StartCoroutine(MoveAlongField(GameManager.LastRolledNumber));
            shouldMove = false;
            moveEnded = true;
        }
    }

    private void CheckUserInfos()
    {
        Room.UserFigures.Where(figure => figure.UserInfo == null).ForEach(figure => figure.CmdSetShouldUpdateInfo());
    }

    [Command(requiresAuthority = false)]
    private void CmdSetShouldUpdateInfo()
    {
        shouldUpdateSelfInfo = true;
        RpcSetShouldUpdateInfo();
    }

    [ClientRpc]
    private void RpcSetShouldUpdateInfo()
    {
        shouldUpdateSelfInfo = true;
    }

    private void UpdateSelfInfo()
    {
        if (!shouldUpdateSelfInfo)
            return;

        var userInfo = Room.GamePlayers.FirstOrDefault(user => user.hasAuthority);
        CmdInitUserInfo(userInfo);
    }

    private void ColorFigure()
    {
        if (UserInfo == null || colored)
            return;

        colored = true;
        UserOutline.OutlineColor = UserInfo.DisplayColor;
    }

    private void InitPlayer()
    {
        if (inited)
            return;
        inited = true;

        CmdSetUserMoney(1500);    
    }

    public IEnumerator MoveAlongField(int unitCount)
    {
        for (int i = Math.Abs(unitCount); i > 0; i--)
        {
            clientPosition += unitCount / Math.Abs(unitCount);
            clientPosition %= Field.fieldUnits.Count;

            OnMoveFigure();

            var nextPos = Field.GetAvailablePointForField(clientPosition);
            yield return Move(nextPos);
            yield return new WaitForSeconds(0.1f);
        }
        AfterMoveFigure();
        yield break;
    }

    public void MoveToUnit(int unitIndex)
    {
        var unitsBeforeGoal = unitIndex > clientPosition?
            unitIndex-clientPosition:
            Field.fieldUnits.Count + unitIndex - currentPosition;

        StartCoroutine(MoveAlongField(unitsBeforeGoal));
    }

    public IEnumerator Move(Vector3 goal)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while ((transform.position = Vector3.MoveTowards(transform.position, goal, Time.fixedDeltaTime * 10)) != goal)
        {
            yield return null;
        }

        isMoving = false;
    }

    public void BeforeMoveFigure()
    {

    }

    public void OnMoveFigure()
    {
        if (clientPosition == 0)
            LoopPased();
    }

    public void AfterMoveFigure()
    {
        CmdChangeCurrentPos(clientPosition);
        MoveOver();
        isMoving = false;
        OnTurnEnded();
        UIController.UnlockCursor();
        frezeFigure = true;
    }

    private void OnTurnEnded()
    {
        GetCurrentUnit().OnPlayerStop(this);
    }

    public IFieldUnit GetCurrentUnit()
        => Field.fieldUnits[clientPosition];

    public void PayToUser(UserFigure figure, int amount)
    {
        this.CmdSetUserMoney(userMoney - amount);
        figure.CmdSetUserMoney(figure.userMoney + amount);
    }

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

        UIHandler.PlayerInfoUI.DrawUserMoney(money);
        Debug.Log($"Draw money changed to {money}");

        if (UIHandler.TabMenuUI.gameObject.activeSelf)
            UIHandler.TabMenuUI.ShowTabMenu(Room.UserFigures);
    }

    [Command(requiresAuthority = false)]
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
    [Command(requiresAuthority = false)]
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

    /// <summary>
    /// Returns gameplayer object assigned to current figure
    /// </summary>
    /// <remarks>
    /// Dont use on client Rpc
    /// </remarks>
    /// <returns></returns>
    [Obsolete]
    public NetworkGamePlayerLobby GetUserInfo()
        => Room.GamePlayers.FirstOrDefault(x => x.connectionToClient == this.connectionToClient);

    #region Outline handle
    private void InitOutlineColors()
    {
        foreach (var figure in Room.UserFigures)
        {
            figure.SetOutline(figure.UserInfo.DisplayColor);
        }
    }

    private void SetOutline(Color color)
    {
        this.UserOutline.OutlineColor = color;
    }
    #endregion
}
