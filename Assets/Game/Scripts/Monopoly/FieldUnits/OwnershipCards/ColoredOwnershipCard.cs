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
    public class ColoredOwnershipCard : OwnershipCardBase
    {
        [SerializeField] TMP_Text[] valueTexts;
        [SerializeField] Image UnitColor;

        public override void InitCard(BuyableFieldUnitBase fieldUnit)
        {
            Field = fieldUnit;

            UnitColor.color = fieldUnit.color.GetColor();
            valueTexts[0].text = fieldUnit.unitName;
            valueTexts[1].text = $"RENT ${fieldUnit.basePayAmount}.";

            for (int i = 0; i < 4; i++)
            {
                valueTexts[i + 2].text = $"{((ImproveableFieldUnit)fieldUnit).improvedRentalPrices[i]}.";
            }

            valueTexts[6].text = $"With HOTEL ${((ImproveableFieldUnit)fieldUnit).improvedRentalPrices[4]}.";
            valueTexts[7].text = $"Mortgage Value ${((ImproveableFieldUnit)fieldUnit).mortgageValue}.";
            valueTexts[8].text = $"Houses Cost ${((ImproveableFieldUnit)fieldUnit).houseCost}. each";
            valueTexts[9].text = $"Hotels, ${((ImproveableFieldUnit)fieldUnit).hotelCost}. plus 4 houses";
        }

        public override void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
