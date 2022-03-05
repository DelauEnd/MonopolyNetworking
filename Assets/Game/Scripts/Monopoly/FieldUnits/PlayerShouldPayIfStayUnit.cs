using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class PlayerShouldPayIfStayUnit : FieldUnitBase
    {
        [SerializeField] public int basePayAmount = 0;

        public override void OnPlayerStop(UserFigure figure)
        {
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.BuildMessage($"You should pay {GetPayAmount()} TAX");
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.ShowUI();
        }

        protected virtual int GetPayAmount()
            => basePayAmount;

        public virtual void PayByPlayer(UserFigure figure)
        {
            var payAmount = GetPayAmount();
            if(figure.userMoney < payAmount)
            {
                UserHasNoMoney();
                return;
            }
            figure.CmdSetUserMoney(figure.userMoney - payAmount);
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.HideUI();
        }

        protected void UserHasNoMoney()
        {
            throw new NotImplementedException();
        }
    }
}
