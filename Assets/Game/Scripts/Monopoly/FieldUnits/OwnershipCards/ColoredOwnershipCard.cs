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
            valueTexts[1].text = $"RENT <sprite index= 0>{fieldUnit.basePayAmount}.";

            for (int i = 0; i < 4; i++)
            {
                valueTexts[i + 2].text = $"{((ImproveableFieldUnit)fieldUnit).improvedRentalPrices[i]}.";
            }
            valueTexts[6].text = $"With HOTEL <sprite index= 0>{((ImproveableFieldUnit)fieldUnit).improvedRentalPrices[4]}.";
            valueTexts[7].text = $"Mortgage Value <sprite index= 0>{((ImproveableFieldUnit)fieldUnit).mortgageValue}.";
            valueTexts[8].text = $"Houses Cost <sprite index= 0>{((ImproveableFieldUnit)fieldUnit).improveCost}. each";
            valueTexts[9].text = $"Hotels, <sprite index= 0>{((ImproveableFieldUnit)fieldUnit).improveCost}. plus 4 houses";

            mortgageCard.InitMortgageCard(fieldUnit);
        }

        public override void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
