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
        public UserOfferPanel YouGivePanel;
        public UserOfferPanel YouRecievePanel;

        public UserFigure offerSender;
        public UserFigure offerReciever;

        public void ShowTradePanel(UserFigure offerReciever)
        {
            this.offerReciever = offerReciever;
            ClearOffer();
            gameObject.SetActive(true);
            YouGivePanel.InitOfferPanel(offerSender);
            YouRecievePanel.InitOfferPanel(this.offerReciever);
        }

        public void HideTradePanel()
        {
            ClearOffer();
            gameObject.SetActive(false);
        }

        private void ClearOffer()
        {
            YouGivePanel.ClearOffer();
            YouRecievePanel.ClearOffer();
        }
    }
}
