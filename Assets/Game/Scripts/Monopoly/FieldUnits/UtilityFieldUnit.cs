using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilityFieldUnit : BuyableFieldUnitBase
{
    public override void OnPlayerStop(UserFigure figure)
    {
        if (AvailableToBuy)
            figure.UIHandler.buyUnitButton.gameObject.SetActive(true);
        else
            ShowPayMenu(figure);
    }

    protected override int GetPayAmount()
    {
        var ownedCount = GetUnitsWithSameColor().Count(unit => ((BuyableFieldUnitBase)unit).owner = this.owner);
        var priceMultiplayer = ownedCount == 1 ?
            6 :
            10;

        //rolled dice amount * multiplayer
        throw new NotImplementedException();

        return basePayAmount * priceMultiplayer;
    }
}