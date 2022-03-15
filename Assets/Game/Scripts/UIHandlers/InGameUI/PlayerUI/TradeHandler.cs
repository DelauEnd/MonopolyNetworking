using Assets.Game.Scripts.Monopoly.TradeBetweenUsers;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.Notification;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI
{
    public class TradeHandler : NetworkBehaviour
    {
        UserFigure Figure = null;
        [SerializeField] GameObject RecievedOffersBaseObject = null;

        private void Awake()
        {
            Figure = GetComponent<UserFigure>();
        }

        public void SendOffer(TradeOfferToSend offer)
        {
            CmdSendOffer(offer);
        }

        [Command(requiresAuthority = false)]
        private void CmdSendOffer(TradeOfferToSend offer)
        {
            RpcSendOffer(offer);
        }

        [ClientRpc]
        private void RpcSendOffer(TradeOfferToSend offer)
        {
            if (Figure.Room.GamePlayers.FirstOrDefault(fig => fig.hasAuthority).UserId == offer.recieverPlayerId)
                Figure.Room.UserFigures.FirstOrDefault(fig => fig.hasAuthority).GetComponent<TradeHandler>().ShowOffer(offer);
        }

        private void ShowOffer(TradeOfferToSend offer)
        {
            var offerInstance = GameObject.Instantiate(Resources.Load("Prefabs/TradePanel")) as GameObject;

            offerInstance.GetComponent<RecieveTradePanel>().ShowOfferPanel(offer);
            offerInstance.transform.SetParent(RecievedOffersBaseObject.transform);
            offerInstance.transform.localScale = Vector3.one;
            offerInstance.transform.localPosition = RecievedOffersBaseObject.transform.position;        
        }

        private void TryGetOffer(TradeOfferToSend offer)
        {
            if (Figure.UserInfo.UserId != offer.recieverPlayerId)
                return;
        }

        public void SendBackMessage(bool accepted)
        {
            var notification = Figure.GetComponent<NotificationHandler>().InstantiateNotification();

            if (accepted)
                notification.ShowOneButtonNotification("User accepted your offer", "Ok");
            else
                notification.ShowOneButtonNotification("User decline your offer", "Ok");
        }

        [Command(requiresAuthority = false)]
        public void CmdSendBackMessage(TradeOffer messageReciever, bool accepted)
        {
            RpcSendBackMessage(messageReciever, accepted);
        }

        [ClientRpc]
        private void RpcSendBackMessage(TradeOffer messageReciever, bool accepted)
        {
            if (Figure.Room.GamePlayers.FirstOrDefault(fig => fig.hasAuthority).UserId == messageReciever.senderPlayerId)
                Figure.Room.UserFigures.FirstOrDefault(fig => fig.hasAuthority).GetComponent<TradeHandler>().SendBackMessage(accepted);
        }
    }
}
