using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList;
using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using Assets.Game.Scripts.Utils.Extensions;

namespace Assets.Game.Scripts.Monopoly.TradeBetweenUsers
{
    public class RecieveOfferPanel : MonoBehaviour
    {
        [SerializeField] GameObject contentPanel = null;      
        [SerializeField] TMP_Text MaxMoney = null;
       
        [SerializeField] public List<BuyableFieldUnitBase> SelectedFields = new List<BuyableFieldUnitBase>();
        [SerializeField] List<GameObject> selectedInstances = new List<GameObject>();

        UserFigure player = null;

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

        public void InitOfferPanel(TradeOffer offer)
        {
            player = Room.UserFigures.FirstOrDefault(user=>user.UserInfo.UserId == offer.senderPlayerId);
            MaxMoney.text = $"<sprite index= 0>{offer.moneyAmount}.";

            foreach (var ind in offer.fieldUnitIndexes)
            {
                AddFieldToSelected((BuyableFieldUnitBase)player.Field.fieldUnits[ind]);
            }
        }

        public void AddFieldToSelected(BuyableFieldUnitBase field)
        {
            if (field.owner != player)
                return;

            SelectedFields.Add(field);

            var recordInstance = GameObject.Instantiate(Resources.Load("Prefabs/Scrolbar/ScrolbarItem")) as GameObject;

            recordInstance.GetComponent<OwnershipListItem>().InitOwnedField(field);
            InitInstance(field, recordInstance);
        }

        private void InitInstance(BuyableFieldUnitBase field, GameObject recordInstance)
        {
            recordInstance.transform.SetParent(contentPanel.transform);

            recordInstance.transform.localScale = Vector3.one;
            recordInstance.transform.localPosition = recordInstance.transform.position;

            selectedInstances.Add(recordInstance);
        }
    }
}