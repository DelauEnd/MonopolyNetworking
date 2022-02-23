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
            GoToJail(figure);
        }

        public void GoToJail(UserFigure figure)
        {
            figure.CmdSetPlayerPrisonRemained(3);
            figure.clientPosition = 10;

            StartCoroutine(MoveToJail(figure));
        }

        IEnumerator MoveToJail(UserFigure figure)
        {
            var usersInJailCount = figure.Room.UserFigures.Count(figure => figure.prisonRemained > 0);

            Transform jailPos = jailPositions[usersInJailCount - 1];

            bool isMoving = false;
            if (isMoving)
            {
                yield break;
            }
            isMoving = true;

            while ((figure.transform.position = Vector3.MoveTowards(figure.transform.position, jailPos.position, Time.fixedDeltaTime*10)) != jailPos.position)
            {
                yield return null;
            }

            isMoving = false;
        }
    }
}
