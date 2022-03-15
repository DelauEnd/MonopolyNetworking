using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.TradeBetweenUsers
{
    public class CreateTradePanel : MonoBehaviour
    {
        public UserOfferPanel RecieverOfferPanel;
        public UserOfferPanel SenderOfferPanel;

        public UserFigure offerSender;
        public UserFigure offerReciever;

        [SerializeField] TradeHandler Handler = null;

        public void ShowTradePanel(UserFigure offerReciever)
        {
            this.offerReciever = offerReciever;
            ClearOffer();
            gameObject.SetActive(true);
            SenderOfferPanel.InitOfferPanel(offerSender);
            RecieverOfferPanel.InitOfferPanel(this.offerReciever);
        }

        public void HideTradePanel()
        {
            ClearOffer();
            gameObject.SetActive(false);
        }

        private void ClearOffer()
        {
            SenderOfferPanel.ClearOffer();
            RecieverOfferPanel.ClearOffer();
        }


        public void SendOffer()
        {
            var offer = BuildOffer();
            Handler.SendOffer(offer);
            HideTradePanel();
        }

        public TradeOfferToSend BuildOffer()
        {
            var recieverOffer = RecieverOfferPanel.BuildOffer();
            var senderOffer = SenderOfferPanel.BuildOffer();

            return new TradeOfferToSend
            {
                recieverPlayerId = offerReciever.UserInfo.UserId,
                senderPlayerId = offerSender.UserInfo.UserId,

                recieverOfferMoney = recieverOffer.moneyAmount,
                recieverOfferUnitsInds = recieverOffer.fieldUnitIndexes,

                senderOfferMoney = senderOffer.moneyAmount,
                senderOfferUnitsInds = senderOffer.fieldUnitIndexes,
            };
        }
    }
}
