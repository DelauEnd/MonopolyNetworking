using Assets.Game.Scripts.Monopoly.FieldUnits;
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
    public OwnerCard ownerCard;
    public int unitPrice = 0;
    public bool AvailableToBuy
        => owner == null;

    public abstract override void OnPlayerStop(UserFigure figure);

    protected void ShowBuyMenu(UserFigure figure)
    {
        figure.UIHandler.buyUnitButton.gameObject.SetActive(true);
    }

    public void BuyByUser(UserFigure figure)
    {
        if (figure.userMoney<unitPrice)
        {
            UserHasNoMoney();
            return;
        }
        figure.RpcSetUserMoney(figure.userMoney - unitPrice);
        owner = figure;
    }

    public void ChangeOwner(NetworkGamePlayerLobby newOwner, UserFigure newOwnerFigure)
    {
        owner = newOwnerFigure;
        ownerCard.SetOwnerInfo(newOwner);
        ownerCard.gameObject.SetActive(true);
    }

    public override void PayByPlayer(UserFigure figure)
    {
        var payAmount = GetPayAmount();
        if (figure.userMoney < payAmount)
        {
            UserHasNoMoney();
            return;
        }
        figure.CmdSetUserMoney(figure.userMoney - payAmount);
        owner.CmdSetUserMoney(owner.userMoney + payAmount);
    }

    protected abstract override int GetPayAmount();
    //{
    //    var ownedCount = GetUnitsWithSameColor().Count(unit => ((BuyableFieldUnit)unit).owner = this.owner);
    //    var priceMultiplayer = (int)Math.Pow(2, ownedCount - 1);

    //    return basePayAmount * priceMultiplayer;
    //}

    protected IEnumerable<IFieldUnit> GetUnitsWithSameColor()
        => Field.fieldUnits.Where(unit => unit is BuyableFieldUnitBase && ((BuyableFieldUnitBase) unit).color == this.color).ToList(); 
}