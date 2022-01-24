using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceCheck : MonoBehaviour
{
    List<Dice> dices;
    List<Vector3> diceVelosities;
    public bool dicesRolled;
    public bool isNumbersCalculated
        => !dices.Any(dice => dice.rolledNumber == 0);

    public int rolledSum 
        => dices.Sum(dice => dice.rolledNumber);

    private void Start()
    {
        dices = new List<Dice>(transform.parent.parent.GetComponentsInChildren<Dice>().Where(dice => dice.transform != this.transform));
    }

    private void FixedUpdate()
    {
        diceVelosities = dices.Select(x => x.diceVelocity).ToList();
    }

    private void OnTriggerStay(Collider obj)
    {
        if (IsDicesRolled())
        {
            var dice = obj.GetComponentInParent<Dice>();
            switch (obj.gameObject.name)
            {
                case "side1":
                    dice.rolledNumber = 2;
                    break;
                case "side2":
                    dice.rolledNumber = 1;
                    break;
                case "side3":
                    dice.rolledNumber = 5;
                    break;
                case "side4":
                    dice.rolledNumber = 6;
                    break;
                case "side5":
                    dice.rolledNumber = 3;
                    break;
                case "side6":
                    dice.rolledNumber = 4;
                    break;
            }
        }
    }

    public bool IsDicesRolled()
         => diceVelosities.All(vector => vector == Vector3.zero);
    

    public void RollAllDices()
    {
        dicesRolled = true;
        foreach (var dice in dices)
        {
            dice.RollDice();
        }
    }

    public void ClearRolledNumbers()
    {
        foreach (var dice in dices)
        {
            dice.rolledNumber = 0;
        }
    }
}
