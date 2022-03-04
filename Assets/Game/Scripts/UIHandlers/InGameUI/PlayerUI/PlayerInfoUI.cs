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

        public void DrawUserMoney(int money)
        {
            Debug.Log($"changed money value to {money}");
            moneyText.text = $"{money}<sprite index= 0>";
        }
    }
}
