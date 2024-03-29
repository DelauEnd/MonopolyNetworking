﻿using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class JustStayFieldUnit : FieldUnitBase
    {
        public override void OnPlayerStop(UserFigure figure)
        {
            figure.UIHandler.GameUnitsPlayerUI.JustStayUnitUI.BuildMessage($"You stay on free field, just CHILL");
            figure.UIHandler.GameUnitsPlayerUI.JustStayUnitUI.ShowUI();
        }
    }
}
