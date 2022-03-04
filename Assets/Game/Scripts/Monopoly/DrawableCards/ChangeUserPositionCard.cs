using Assets.Game.Scripts.Monopoly.Enums;
using Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.DrawableCards
{
    public class ChangeUserPositionCard : DrawableCardBase
    {
        public int moveAmount = 0;

        public override DrawableCardType CardType
            => DrawableCardType.MovePlayer;

        public override void OnUserDrawCard(UserFigure figure)
        {
            StartCoroutine(figure.MoveAlongField(moveAmount));
        }
    }
}