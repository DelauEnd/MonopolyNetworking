using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit
{
    public interface IDrawableCard
    {
        public void OnUserDrawCard(UserFigure figure);
    }
}
