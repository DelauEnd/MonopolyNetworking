using Assets.Game.Scripts.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards
{
    public class RailroadOwnershipCard : OwnershipCardBase
    {
        [SerializeField] TMP_Text nameText;

        public override void InitCard(BuyableFieldUnitBase fieldUnit)
        {
            Field = fieldUnit;
            nameText.text = fieldUnit.unitName;

            mortgageCard.InitMortgageCard(fieldUnit);
        }

        public override void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
