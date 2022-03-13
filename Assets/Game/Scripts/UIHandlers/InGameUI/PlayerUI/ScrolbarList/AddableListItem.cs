using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Assets.Game.Scripts.Monopoly.TradeBetweenUsers;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList
{
    public class AddableListItem : FieldListItemBase
    {
        [SerializeField] public OwnershipListGUI Container = null;

        [SerializeField] public UserOfferPanel userOfferPanel = null;
        [SerializeField] Text FieldName = null;


        public void AddItem()
        {
            userOfferPanel.AddFieldToSelected(UnitField);
            Container.HideOwnershipList();
        }

        public override void InitOwnedField(BuyableFieldUnitBase fieldUnit)
        {
            FieldName.text = fieldUnit.unitName;
            UnitField = fieldUnit;
        }
    }
}
