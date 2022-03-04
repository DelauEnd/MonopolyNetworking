using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards;
using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BuyableFieldUnitBase : PlayerShouldPayIfStayUnit
{
    public UserFigure owner = null;
    public UnitColor color;
    public string unitName;
    public OwnershipCardBase ownerCard = null;
    public int unitPrice = 0;
    public int mortgageValue = 0;
    public bool AvailableToBuy
        => owner == null;

    protected override void Awake()
    {
        base.Awake();
        if (ownerCard != null)
            ownerCard.InitCard(this);
    }

    public abstract override void OnPlayerStop(UserFigure figure);

    public void BuyField(UserFigure newOwner)
    {
        CmdBuyCurrentField(newOwner);
    }

    [Command(requiresAuthority = false)]
    public void CmdBuyCurrentField(UserFigure figure)
    {
        if (figure.userMoney < unitPrice)
        {
            UserHasNoMoney();
            return;
        }
        figure.RpcSetUserMoney(figure.userMoney - unitPrice);
        owner = figure;

        figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.HideUI(); ;
        RpcBuyCurrentField(figure.UserInfo, figure);
    }

    public override void PayByPlayer(UserFigure figure)
    {
        var payAmount = GetPayAmount();
        if (figure.userMoney < payAmount)
        {
            UserHasNoMoney();
            return;
        }
        figure.PayToUser(owner, payAmount);

        //figure.UIHandler.payRentaButton.gameObject.SetActive(false);
    }

    [ClientRpc]
    private void RpcBuyCurrentField(NetworkGamePlayerLobby newOwner, UserFigure newOwnerFigure)
    {
        owner = newOwnerFigure;
        if (ownerCard != null)
        {
            ownerCard.SetVisible(true);
            ownerCard.SetNewOwner(newOwnerFigure);
        }

        newOwnerFigure.UIHandler.GameUnitsPlayerUI.EndTurn();
    }

    protected abstract override int GetPayAmount();

    protected IEnumerable<IFieldUnit> GetUnitsWithSameColor()
        => Field.fieldUnits.Where(unit => unit is BuyableFieldUnitBase && ((BuyableFieldUnitBase) unit).color == this.color).ToList(); 
}