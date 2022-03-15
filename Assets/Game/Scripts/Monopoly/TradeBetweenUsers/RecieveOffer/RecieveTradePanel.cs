using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.TradeBetweenUsers
{
    public class RecieveTradePanel : MonoBehaviour
    {
        public RecieveOfferPanel RecieverOfferPanel;
        public RecieveOfferPanel SenderOfferPanel;

        private TradeOffer recieverOffer;
        private TradeOffer senderOffer;

        private NetworkManagerLobby room;
        public NetworkManagerLobby Room
        {
            get
            {
                if (room != null)
                    return room;
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public void ShowOfferPanel(TradeOfferToSend offer)
        {
            recieverOffer = new TradeOffer
            {
                 fieldUnitIndexes =offer.recieverOfferUnitsInds,
                 moneyAmount = offer.recieverOfferMoney,
                 senderPlayerId = offer.recieverPlayerId
            };

            senderOffer = new TradeOffer
            {
                fieldUnitIndexes = offer.senderOfferUnitsInds,
                moneyAmount = offer.senderOfferMoney,
                senderPlayerId = offer.senderPlayerId
            };

            SenderOfferPanel.InitOfferPanel(senderOffer);
            RecieverOfferPanel.InitOfferPanel(recieverOffer);
        }

        public void AcceptOffer()
        {
            if (!(IsOfferValid(recieverOffer) && IsOfferValid(senderOffer)))
                return;

            ApplyOffer(recieverOffer, senderOffer);
            ApplyOffer(senderOffer, recieverOffer);
            room.UserFigures[0].GetComponent<TradeHandler>().CmdSendBackMessage(senderOffer, true);

            Destroy(this.gameObject);
        }

        private void ApplyOffer(TradeOffer senderOffer, TradeOffer recieverOffer)
        {
            var figure = Room.UserFigures.FirstOrDefault(user => user.UserInfo.UserId == recieverOffer.senderPlayerId);

            var moneyDiff = senderOffer.moneyAmount - recieverOffer.moneyAmount;
            figure.CmdSetUserMoney(figure.userMoney + moneyDiff);

            foreach (var ind in senderOffer.fieldUnitIndexes)
            {
                ((BuyableFieldUnitBase)figure.Field.fieldUnits[ind]).CmdChangeOwner(figure);
            }           
        }

        private bool IsOfferValid(TradeOffer offer)
        {
            var figure = Room.UserFigures.FirstOrDefault(user => user.UserInfo.UserId == offer.senderPlayerId);

            if (figure.userMoney < offer.moneyAmount)
                return false;

            foreach (var ind in offer.fieldUnitIndexes)
            {
                if (((BuyableFieldUnitBase)figure.Field.fieldUnits[ind]).owner != figure)
                    return false;
            }

            return true;
        }

        public void CancelOffer()
        {
            room.UserFigures[0].GetComponent<TradeHandler>().CmdSendBackMessage(senderOffer, false);
            Destroy(this.gameObject);
        }
    }
}
