using System;
using System.Linq;

public class ImproveableFieldUnit : BuyableFieldUnitBase
{
    public int maxImproveLevel;
    public int currentImproveLevel = 0;
    public int houseCost;
    public int hotelCost;
    public int[] improvedRentalPrices = new int[5];

    public bool UserCanImproveField(UserFigure figure)
        => (owner == figure && GetUnitsWithSameColor().All(unit => ((BuyableFieldUnitBase)unit).owner == owner));

    public override void OnPlayerStop(UserFigure figure)
    {
        if (AvailableToBuy)
            ShowBuyMenu(figure);
        else if (UserCanImproveField(figure))
            ; // user can improve unit if he wants
        else
            ShowPayMenu(figure);
    }

    protected override int GetPayAmount()
    {
        if (currentImproveLevel == 0)
        {
            if (GetUnitsWithSameColor().All(unit => ((BuyableFieldUnitBase)unit).owner == owner))
                return basePayAmount * 2;
            else
                return basePayAmount;
        }
        else
            return improvedRentalPrices[currentImproveLevel - 1];
    }


}