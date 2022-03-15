using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceCheck : MonoBehaviour
{
    [SerializeField] List<Dice> dices = null;
    List<Vector3> diceVelosities;
    public bool dicesRolled;
    public bool IsNumbersCalculated
        => !dices.Any(dice => dice.rolledNumber == 0);

    public int RolledSum 
        => dices.Sum(dice => dice.rolledNumber);

    private void Awake()
    {
        //dices = new List<Dice>(FindObjectsOfType<Dice>());
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
                    dice.rolledNumber = 6;
                    break;
                case "side2":
                    dice.rolledNumber = 5;
                    break;
                case "side3":
                    dice.rolledNumber = 4;
                    break;
                case "side4":
                    dice.rolledNumber = 3;
                    break;
                case "side5":
                    dice.rolledNumber = 2;
                    break;
                case "side6":
                    dice.rolledNumber = 1;
                    break;
                case "edge":
                    dice.GetComponent<Rigidbody>().AddForce(Vector3.up);
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
