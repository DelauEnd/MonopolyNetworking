using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RailroadFieldUnit : BuyableFieldUnitBase
{
    public override void OnPlayerStop(UserFigure figure)
    {
        if (AvailableToBuy)
            figure.UIHandler.GameUnitsPlayerUI.BuyableUnitUI.ShowUI();
        else
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.ShowUI();
    }

    protected override int GetPayAmount()
    {
        var ownedCount = GetUnitsWithSameColor().Count(unit => ((BuyableFieldUnitBase)unit).owner = this.owner);
        var priceMultiplayer = (int)Math.Pow(2, ownedCount - 1);

        return basePayAmount * priceMultiplayer;
    }
}