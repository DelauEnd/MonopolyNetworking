using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LinkedList<UserFigure> users = null;
    public static LinkedListNode<UserFigure> currentUser = null;
    public static LinkedListNode<UserFigure> previousUser = null;

    public DiceCheck dices;

    private void Start()
    {
        dices = GetComponentInChildren<DiceCheck>();
    }

    public static void InitGame()
    {
        users = new LinkedList<UserFigure>(FindObjectsOfType<UserFigure>());
        currentUser = users.First;
        previousUser = users.Last;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !dices.dicesRolled && !currentUser.Value.isMoving && !previousUser.Value.isMoving)
        //{
        //    dices.ClearRolledNumbers();
        //    dices.RollAllDices();
        //}

        //if (dices.isNumbersCalculated && dices.dicesRolled)
        //{
        //    dices.dicesRolled = false;
        //    currentUser.Value.steps = dices.rolledSum;
        //    Debug.Log("Dice rolled: " + currentUser.Value.steps);
        //    currentUser.Value.shouldMove = true;
        //    NextUser();
        //}
    }

    public void NextUser()
    {
        previousUser = currentUser;
        currentUser = currentUser.Next ?? users.First;     
    }

    private void FixedUpdate()
    {
        
    }
}
