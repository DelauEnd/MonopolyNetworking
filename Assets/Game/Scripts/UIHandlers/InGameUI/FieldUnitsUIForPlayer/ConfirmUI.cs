using Assets.Game.Scripts.Monopoly.FieldUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.FieldUnitsUIForPlayer
{
    public class ConfirmUI : UnitUIBase
    {
        [SerializeField] Button ConfirmButton = null;

        public override void BuildMessage(string message, string confirmButtonText = null, string button2Text = null)
        {
            MessageText.text = message;

            if (confirmButtonText != null)
                ConfirmButton.GetComponentInChildren<Text>().text = confirmButtonText;
        }

        public void Confirm()
        {
            ((GoToJailFieldUnit)Figure.GetCurrentUnit()).MovePlayerToJail(Figure);
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
