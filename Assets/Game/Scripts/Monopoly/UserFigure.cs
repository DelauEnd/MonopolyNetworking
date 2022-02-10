using Assets.Game.Scripts.Utils;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserFigure : NetworkBehaviour
{  
    public GameField field = null;

    [SyncVar(hook = nameof(SyncCurrentPos))] 
    public int currentPosition = 0;
    private int clientPosition = 0;

    public bool shouldMove;
    public int steps = 0;
    public uint userMoney = 1500;
    public bool isMoving;
    public bool moveEnded;

    //TODO: Store all player figures into the Gamefield as static List<UserFigure>. After sync update value in list via userlist[current] == this

    private void Start()
    {
        field = FindObjectOfType<GameField>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasAuthority)
        {
            shouldMove = true;
            steps = 1;
        }

        if (shouldMove && hasAuthority)
        {
            moveEnded = false;
            StartCoroutine(Move());
            shouldMove = false;
            moveEnded = true;
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

            var nextPos = field.GetAvailablePointForField(clientPosition);
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
    }


    #region CurrentPositionHandle
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
