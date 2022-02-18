using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class PlayerShouldPayIfStayUnit : FieldUnitBase
    {
        protected int basePayAmount = 0;

        public override void OnPlayerStop(UserFigure figure)
        {
            throw new NotImplementedException();
        }

        protected virtual int GetPayAmount()
            => basePayAmount;

        public virtual void PayByPlayer(UserFigure figure)
        {
            figure.CmdSetUserMoney(figure.userMoney - GetPayAmount());
        }
    }
}
