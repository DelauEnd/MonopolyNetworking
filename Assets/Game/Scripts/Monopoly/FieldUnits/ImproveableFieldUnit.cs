using Assets.Game.Scripts.Monopoly.FieldUnits;
using System;
using System.Linq;
using UnityEngine;

public class ImproveableFieldUnit : BuyableFieldUnitBase
{
    public readonly int MaxImproveLevel = 5;
    public int improveCost;
    public int[] improvedRentalPrices = new int[5];

    [Header("Unit state")]
    public int currentImproveLevel = 0;
    public UnitBuildings buildings = null;

    public bool UserCanImproveField(UserFigure figure)
        => UserOwnsAllColorFields(figure)
        && !GetUnitsWithSameColor().Any(unit => ((ImproveableFieldUnit)unit).currentImproveLevel < currentImproveLevel);

    public bool UserCanWorsenField(UserFigure figure)
        => UserOwnsAllColorFields(figure)
        && !GetUnitsWithSameColor().Any(unit => ((ImproveableFieldUnit)unit).currentImproveLevel > currentImproveLevel);

    bool UserOwnsAllColorFields(UserFigure figure)
        => (owner == figure && GetUnitsWithSameColor().All(unit => ((BuyableFieldUnitBase)unit).owner == owner));

    public override void OnPlayerStop(UserFigure figure)
    {
        if (AvailableToBuy)
        {
            figure.UIHandler.GameUnitsPlayerUI.BuyableUnitUI.BuildMessage($"{unitName}\nYou can buy this field for {unitPrice}$ ");
            figure.UIHandler.GameUnitsPlayerUI.BuyableUnitUI.ShowUI();
        }
        else
        {
            figure.UIHandler.GameUnitsPlayerUI.BuyableUnitUI.BuildMessage($"{unitName}\nOwner: {owner.UserInfo.DisplayName}.\nYou should pay Renta {GetPayAmount()}$ ");
            figure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.ShowUI();
        }

    }

    protected override int GetPayAmount()
    {
        if (currentImproveLevel == 0)
        {
            if (GetUnitsWithSameColor().All(unit => ((BuyableFieldUnitBase)unit).owner != null && ((BuyableFieldUnitBase)unit).owner == owner))
                return basePayAmount * 2;
            else
                return basePayAmount;
        }
        else
            return improvedRentalPrices[currentImproveLevel - 1];
    }

    public void ImproveUnit(UserFigure user)
    {
        if (!UserCanImproveField(user) || currentImproveLevel == 5)
            return;

        user.CmdSetUserMoney(user.userMoney - improveCost);
        currentImproveLevel++;
        buildings.UpdateBuildings(currentImproveLevel);
    }

    public void SoldBuilding(UserFigure user)
    {
        if (owner != user || currentImproveLevel == 0)
            return;

        user.CmdSetUserMoney(user.userMoney + improveCost / 2);
        currentImproveLevel--;
        buildings.UpdateBuildings(currentImproveLevel);
    }
}