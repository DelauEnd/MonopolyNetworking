using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards
{
    public class MortgageCard : MonoBehaviour
    {
        public TMP_Text[] texts = new TMP_Text[2];

        public void InitMortgageCard(BuyableFieldUnitBase baseUnit)
        {
            texts[0].text = baseUnit.unitName;
            texts[1].text = $"MORTGAGED\nfor <sprite index= 0>{baseUnit.mortgageValue}.";
        }
    }
}
