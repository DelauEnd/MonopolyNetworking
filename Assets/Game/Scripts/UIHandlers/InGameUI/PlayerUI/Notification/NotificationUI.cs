using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI
{
    public class NotificationUI : MonoBehaviour
    {
        [SerializeField] TMP_Text messageText = null;
        [SerializeField] Text[] buttonTexts;

        [SerializeField] GameObject TwoButtonsConfirm = null;
        [SerializeField] GameObject OneButtonConfirm = null;

        Action onConfirm = null;
        Action onDecline = null;

        public void ShowOneButtonNotification(string message, string buttonText = "Confirm", Action confirmAction = null)
        {
            gameObject.SetActive(true);
            OneButtonConfirm.SetActive(true);
            onConfirm = confirmAction;

            messageText.text = message;
            buttonTexts[0].text = buttonText;
        }

        public void ShowTwoButtonNotification(string message, string confirmText = "Ok", string declineText = "Cancel", Action confirmAction = null, Action declineAction = null)
        {
            gameObject.SetActive(true);
            TwoButtonsConfirm.SetActive(true);
            onConfirm = confirmAction;
            onDecline = declineAction;

            messageText.text = message;
            buttonTexts[1].text = confirmText;
            buttonTexts[2].text = declineText;
        }

        public void Confirm()
        {
            onConfirm?.Invoke();
            DisableAll();
        }

        public void Decline()
        {
            onDecline?.Invoke();
            DisableAll();
        }

        void DisableAll()
        {
            gameObject.SetActive(false);
            OneButtonConfirm.SetActive(false);
            TwoButtonsConfirm.SetActive(false);
        }
    }
}
