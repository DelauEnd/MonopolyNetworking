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
    public class UserGetMoneyFromOtherPlayersCard : DrawableCardBase
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

                user.CmdSetUserMoney(user.userMoney - changeMoneyAmount);
                user.NotificateUser($"You should pay <sprite index= 0>{changeMoneyAmount}. to {figure.UserInfo.DisplayName}", "Confirm");
                finalSum += changeMoneyAmount;
            }

            figure.CmdSetUserMoney(figure.userMoney + finalSum);
            figure.NotificateUser($"You recieve <sprite index= 0>{finalSum}. from other players", "Confirm");
        }
    }
}
