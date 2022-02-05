using Assets.Game.Scripts.Utils;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFigure : NetworkBehaviour
{
    public GameField gameField = null;
    public int currentPosition = 0;
    public int steps;
    public uint userMoney = 1500;
    public bool isMoving;
    public bool shouldMove;
    public bool moveEnded;

    float anim;

    private void Update()
    {
        gameField ??= FindObjectOfType<GameField>();
        if(hasAuthority && Input.GetButton(KeyCode.Space.ToString()))
        {
            shouldMove = true;
            steps = 2;
        }
    }

    private void FixedUpdate()
    {
        if (shouldMove)
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
            currentPosition++;
            currentPosition %= gameField.fieldUnits.Count;

            if (currentPosition == 0)
                LoopPased();

            var nextPos = gameField.fieldUnits[currentPosition].GetAvailablePoint();
            while (ShouldMoveToNext(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            anim = 0;
            steps--;

            gameField.fieldUnits[currentPosition].lockedPoints++;

            var previousUnitIndex = currentPosition == 0 ?
                gameField.fieldUnits.Count - 1 :
                currentPosition - 1;
            gameField.fieldUnits[previousUnitIndex].lockedPoints--;
        }

        MoveOver();
        isMoving = false;
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
        transform.position = Vector3.MoveTowards(transform.position, goal, Time.fixedDeltaTime);

        return goal != transform.position;
    }
}
