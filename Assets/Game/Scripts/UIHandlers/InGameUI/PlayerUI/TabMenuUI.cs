using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Game.Scripts.Utils.Extensions;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI
{
    public class TabMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject[] UserInfoPanel = new GameObject[5];
        [SerializeField] TMP_Text[] UsernameTexts = new TMP_Text[5];
        [SerializeField] TMP_Text[] UserMoneyTexts = new TMP_Text[5];
        [SerializeField] RawImage[] UserColors = new RawImage[5];

        public void ShowTabMenu(List<UserFigure> players)
        {
            this.gameObject.SetActive(true);

            var sortedPlayers = players.OrderByDescending(player=>player.userMoney).ToList();

            UserInfoPanel.ForEach(panel=>panel.SetActive(false));

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                UserInfoPanel[i].SetActive(true);
                UsernameTexts[i].text = sortedPlayers[i].UserInfo.DisplayName;
                UserMoneyTexts[i].text = $"${sortedPlayers[i].userMoney.ToString()}";
                UserColors[i].color = sortedPlayers[i].UserInfo.DisplayColor;
            }
        }

        public void HideTabMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
