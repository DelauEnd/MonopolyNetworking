using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit
{
    public interface IDrawCard
    {
        public IEnumerable<IDrawableCard> DrawableCards { get; }
        public IDrawableCard GetRandomCard();
        public void InitCards();
    }
}
