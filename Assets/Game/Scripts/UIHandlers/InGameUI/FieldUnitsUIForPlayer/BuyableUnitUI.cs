using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.FieldUnitsUIForPlayer
{
    public class BuyableUnitUI : UnitUIBase
    {
        [SerializeField] Button BuyButton = null;
        [SerializeField] Button SkipButton = null;

        public override void HideUI()
        {
            gameObject.SetActive(false);
        }

        public override void ShowUI()
        {
            gameObject.SetActive(true);
        }

        public override void BuildMessage(string message, string buttonText1 = null, string buttonText2 = null)
        {
            MessageText.text = message;

            if (buttonText1 != null)
                BuyButton.GetComponentInChildren<Text>().text = buttonText1;

             if (buttonText2 != null)
                SkipButton.GetComponentInChildren<Text>().text = buttonText2;   
        }

        public void BuyCurrentUnit()
        {
            ((BuyableFieldUnitBase)Figure.GetCurrentUnit()).BuyField(Figure);
            EndTurn();
            HideUI();
        }

        public void SkipUnit()
        { 
            EndTurn();
            HideUI();
        }
    }
}
