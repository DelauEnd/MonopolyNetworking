using Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit;
using Assets.Game.Scripts.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class DrawChanceCardFieldUnit : DrawCardFieldBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnPlayerStop(UserFigure figure)
        {
            figure.UIHandler.GameUnitsPlayerUI.DrawCardUI.BuildMessage("You should draw \"CHANCE\" card");
            figure.UIHandler.GameUnitsPlayerUI.DrawCardUI.ShowUI();
        }
    }
}
