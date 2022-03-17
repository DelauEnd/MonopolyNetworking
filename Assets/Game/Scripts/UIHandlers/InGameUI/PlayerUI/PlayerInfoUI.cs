using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;
using System;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.Monopoly.FieldUnits;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private UserFigure Player = null;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private GameObject UserControlPanel = null;

        [SerializeField] private GameObject YourTurnPanel = null;
        [SerializeField] private GameObject OtherTurnPanel = null;
        [SerializeField] private TMP_Text[] TurnTexts;
        bool showControls = true;
        public bool Inited
            => YourTurnPanel.activeSelf || OtherTurnPanel.activeSelf;

        public void ShowYourTurnPanel()
        {
            OtherTurnPanel.SetActive(false);
            YourTurnPanel.SetActive(true);
        }

        public void ShowOtherTurnPanel(string username)
        {
            TurnTexts[1].text = $"{username}'s turn";
            OtherTurnPanel.SetActive(true);
            YourTurnPanel.SetActive(false);
        }

        public void DrawUserMoney(int money)
        {
            Debug.Log($"changed money value to {money}");
            moneyText.text = $"{money}<sprite index= 0>";
            if(money < 0)
            {
                moneyText.color = Color.red;
                Player.NotificateUser("you have a debt, pay it off", "confirm");
            }
            else
            {
                moneyText.color = Color.white;
            }

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                showControls = !showControls;
                UserControlPanel.gameObject.SetActive(showControls);
            }
        }
    }
}
