using Assets.Game.Scripts.Monopoly.TradeBetweenUsers;
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
                Debug.Log("successfull");
        }

        private void TryGetOffer(TradeOfferToSend offer)
        {
            if (Figure.UserInfo.UserId != offer.recieverPlayerId)
                return;
        }
    }
}
