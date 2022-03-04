using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.FieldUnitsUIForPlayer
{
    public abstract class UnitUIBase : MonoBehaviour
    {
        [SerializeField] protected UserFigure Figure = null;
        [SerializeField] protected TMP_Text MessageText = null;

        public abstract void ShowUI();
        public abstract void HideUI();
        protected void EndTurn()
        {
            var newInd = Figure.GetNextPlayerIndex();
            Debug.Log($"New user ind {newInd}");

            Figure.CmdSetCurrentPlayerInd(newInd);
            Figure.frezeFigure = false;
            Figure.UIController.LockCursor();
        }

        public abstract void BuildMessage(string message, string buttonText1 = null, string buttonText2 = null);
    }
}
