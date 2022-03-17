using Assets.Game.Scripts.Monopoly.Enums;
using Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.DrawableCards
{
    public class UserPayToOtherPlayersCard : DrawableCardBase
    {
        public int changeMoneyAmount = 0;

        public override DrawableCardType CardType 
            => DrawableCardType.ChangePlayerMoney;

        public override void OnUserDrawCard(UserFigure figure)
        {
            var finalSum = 0;
            foreach (var user in figure.Room.UserFigures)
            {
                if (user == figure)
                    continue;
                user.NotificateUser($"You recieve <sprite index= 0>{changeMoneyAmount}. from {figure.UserInfo.DisplayName}", "Confirm");
                user.CmdSetUserMoney(user.userMoney + changeMoneyAmount);
                finalSum += changeMoneyAmount;
            }

            figure.CmdSetUserMoney(figure.userMoney - finalSum);
            figure.NotificateUser($"You payed <sprite index= 0>{changeMoneyAmount}. to each player", "Confirm");
        }
    }
}
