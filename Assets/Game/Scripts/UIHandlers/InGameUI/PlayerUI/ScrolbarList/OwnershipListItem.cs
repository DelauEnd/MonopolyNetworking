using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList
{
    public class OwnershipListItem : FieldListItemBase
    {
        [SerializeField] TMP_Text fieldName = null;

        public override void InitOwnedField(BuyableFieldUnitBase fieldUnit)
        {
            UnitField = fieldUnit;
            fieldName.text = fieldUnit.unitName;
        }
    }
}
