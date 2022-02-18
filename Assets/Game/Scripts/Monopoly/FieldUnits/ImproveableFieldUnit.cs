using System;
using System.Linq;

public class ImproveableFieldUnit : BuyableFieldUnit
{
    public int maxImproveLevel;
    public int currentImproveLevel = 0;
    public int[] improvePrices;
    public int[] rentalPrices;

    public bool UserCanImproveField(UserFigure figure)
        => (owner == figure && GetUnitsWithSameColor().All(unit => ((BuyableFieldUnit)unit).owner == owner));

    public override void OnPlayerStop(UserFigure figure)
    {
        if (AvailableToBuy)
            figure.UIHandler.buyUnitButton.gameObject.SetActive(true);
        else if (UserCanImproveField(figure))
            ; // user can improve unit if he wants
        else
            ;// user should pay renta
    }

    private void UserHasNoMoney()
    {
        throw new NotImplementedException();
    }

    protected override int GetPayAmount()
    {
        if (currentImproveLevel == 0)
            return basePayAmount;
        else
            return rentalPrices[currentImproveLevel - 1];
    }


}