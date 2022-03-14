using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Monopoly.TradeBetweenUsers
{
    public struct TradeOfferToSend
    {
        public string senderPlayerId;
        public int[] senderOfferUnitsInds;
        public int senderOfferMoney;

        public string recieverPlayerId;
        public int[] recieverOfferUnitsInds;
        public int recieverOfferMoney;
    }
}
