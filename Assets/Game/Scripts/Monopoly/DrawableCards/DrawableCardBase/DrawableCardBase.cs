using Assets.Game.Scripts.Monopoly.Enums;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit
{
    public abstract class DrawableCardBase : MonoBehaviour
    {
        public abstract void OnUserDrawCard(UserFigure figure);
        public abstract DrawableCardType CardType { get; }
    }
}
