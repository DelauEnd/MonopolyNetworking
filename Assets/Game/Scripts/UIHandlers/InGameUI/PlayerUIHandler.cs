using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;
using System;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.Monopoly.FieldUnits;

public class PlayerUIHandler : NetworkBehaviour
{
    public Canvas UserUI = null;
    [SerializeField] private UserFigure Player = null;

    [Header("UI blocks")]
    [Header("User info")]
    [SerializeField] public GameObject UserInfoPanel = null;
    [SerializeField] private TMP_Text moneyText;

    [Header("Buy unit")]
    [SerializeField] public GameObject FieldInfoPanel = null;
    [SerializeField] public Button buyUnitButton;
    [SerializeField] public Button payRentaButton;
    [SerializeField] public Button endTurnButton;

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    [ClientCallback]
    private void OnEnable()
    {
        UserUI.enabled = true;
    }

    [ClientCallback]
    private void OnDisable()
    {
        UserUI.enabled = false;
    }

    public void EndTurn()
    {
        FieldInfoPanel.SetActive(false);

        var newInd = Player.GetNextPlayerIndex();
        Debug.Log($"New user ind {newInd}");

        Player.CmdCurrentPlayerToNext(newInd);
        Player.frezeFigure = false;
        Player.UIController.LockCursor();
    }

    public void DrawUserMoney(int money)
    {
        Debug.Log($"changed money value to {money}");
        moneyText.text = $"{money}<sprite index= 0>";
    }

    [Command]
    public void CmdBuyCurrentField()
    {
        ((BuyableFieldUnitBase)Player.Field.fieldUnits[Player.currentPosition]).BuyByUser(Player);

        RpcBuyCurrentField(Player.GetUserInfo(), Player);
    }

    [ClientRpc]
    private void RpcBuyCurrentField(NetworkGamePlayerLobby newOwner, UserFigure newOwnerFigure)
    {
        ((BuyableFieldUnitBase)Player.Field.fieldUnits[Player.currentPosition]).ChangeOwner(newOwner, newOwnerFigure);
        EndTurn();
    }

    public void PayRenta()
    {
        ((PlayerShouldPayIfStayUnit)Player.Field.fieldUnits[Player.currentPosition]).PayByPlayer(Player);
        endTurnButton.interactable = true;
    }
}
