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
            ShowPayMenu(figure);
        }

        protected void ShowPayMenu(UserFigure figure)
        {
            figure.UIHandler.endTurnButton.interactable = false;
            figure.UIHandler.payRentaButton.gameObject.SetActive(true);
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
            figure.UIHandler.payRentaButton.gameObject.SetActive(false);
        }

        protected void UserHasNoMoney()
        {
            throw new NotImplementedException();
        }
    }
}
