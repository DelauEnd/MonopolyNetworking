using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class GoToJailFieldUnit : FieldUnitBase
    {
        [SerializeField] public Transform[] jailPositions = new Transform[5];
        public int playersInJail = 0;

        public override void OnPlayerStop(UserFigure figure)
        {
            figure.UIHandler.GameUnitsPlayerUI.ConfirmUI.ShowUI();
        }

        public void MovePlayerToJail(UserFigure figure)
        {
            var usersInJailCount = figure.Room.UserFigures.Count(figure => figure.prisonRemained > 0);
            Transform jailPos = jailPositions[usersInJailCount - 1];
            StartCoroutine(figure.Move(jailPos.position));
    
            figure.CmdSetPlayerPrisonRemained(3);
            figure.clientPosition = 10;
            figure.UIHandler.GameUnitsPlayerUI.EndTurn();
        }
    }
}
