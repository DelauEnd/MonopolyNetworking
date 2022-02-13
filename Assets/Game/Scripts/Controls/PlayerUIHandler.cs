using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    public Canvas UserUI = null;
    [SerializeField] private UserFigure Player = null;

    [Header("UI blocks")]
    [Header("User info")]
    [SerializeField] public GameObject UserInfoPanel = null;
    [SerializeField] private TMP_Text moneyText;

    [Header("Buy unit")]
    [SerializeField] public GameObject BuyUnitPanel = null;
    [SerializeField] private Button endTurnButton;

    public void EndTurn()
    {
        BuyUnitPanel.SetActive(false);

        var newInd = Player.GetNextPlayerIndex();
        Debug.Log($"New user ind {newInd}");

        Player.CmdCurrentPlayerToNext(newInd);
        Player.frezeFigure = false;
        Player.UIController.LockCursor();
    }

    public void DrawUserMoney(int money)
    {
        Debug.Log($"changed money value to {money}");
        moneyText.text = $"Money {money}";
    }

}
