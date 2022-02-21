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
    public class DrawChanceCardFieldUnit : FieldUnitBase, IDrawCard
    {
        public IEnumerable<IDrawableCard> DrawableCards => throw new NotImplementedException();

        public IDrawableCard GetRandomCard()
        {
            throw new NotImplementedException();
        }

        public void InitCards()
        {
            throw new NotImplementedException();
        }

        public override void OnPlayerStop(UserFigure figure)
        {

        }
    }
}
