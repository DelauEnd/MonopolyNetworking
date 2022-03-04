using Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class DrawCommunityChestCardFieldUnit : DrawCardFieldBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnPlayerStop(UserFigure figure)
        {
            figure.UIHandler.GameUnitsPlayerUI.DrawCardUI.ShowUI();
        }
    }
}
