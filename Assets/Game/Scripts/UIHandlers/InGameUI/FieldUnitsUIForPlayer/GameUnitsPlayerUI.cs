using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.UIHandlers.InGameUI.FieldUnitsUIForPlayer;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class GameUnitsPlayerUI : MonoBehaviour
    {
        [SerializeField] public BuyableUnitUI BuyableUnitUI = null;
        [SerializeField] public ConfirmUI ConfirmUI = null;
        [SerializeField] public JustStayUnitUI JustStayUnitUI = null;
        [SerializeField] public DrawCardUI DrawCardUI = null;
        [SerializeField] public PayIfStayUnitUI payIfStayUnitUI = null;

        public void EndTurn()
        {
            JustStayUnitUI.StayConfirm();
        }
    }
}
