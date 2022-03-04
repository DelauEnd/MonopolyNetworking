using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.FieldUnitsUIForPlayer
{
    public class JustStayUnitUI : UnitUIBase
    {
        [SerializeField] Button ConfirmButton = null;

        public override void BuildMessage(string message, string buttonText1 = null, string buttonText2 = null)
        {
            MessageText.text = message;

            if(buttonText1 != null)
                ConfirmButton.GetComponentInChildren<Text>().text = buttonText1;
        }

        public void StayConfirm()
        {
            EndTurn();
            HideUI();
        }

        public override void HideUI()
        {
            gameObject.SetActive(false);
        }

        public override void ShowUI()
        {
            gameObject.SetActive(true);
        }
    }
}
