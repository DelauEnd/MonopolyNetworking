using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.Notification;
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
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.BuildMessage($"You should pay <sprite index= 0>{GetPayAmount()}. TAX","PAY TAX");
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.ShowUI();
        }

        protected virtual int GetPayAmount()
            => basePayAmount;

        public virtual bool PayByPlayer(UserFigure figure)
        {
            var payAmount = GetPayAmount();
            if(figure.userMoney < payAmount)
            {
                UserHasNoMoney(figure, payAmount, new Action(() => { PayByPlayer(figure);}));
                return false;
            }
            figure.CmdSetUserMoney(figure.userMoney - payAmount);
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.HideUI();
            figure.UIHandler.GameUnitsPlayerUI.EndTurn();
            return true;
        }

        protected void UserHasNoMoney(UserFigure figure, int requiredMoney, Action action)
        {
            figure.NotificateUser("You have no enought money to pay. You can offer exchange to other players or mortgate youre fields", "confirm");
            var notif = figure.gameObject.GetComponent<NotificationHandler>().InstantiateNotification(1);
            notif.GetComponent<RectTransform>().sizeDelta = new Vector2(550,300);
            notif.ShowOneButtonNotification($"Money count to pay: {requiredMoney}","Pay", action);
        }
    }
}
