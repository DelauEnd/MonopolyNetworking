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

    public bool mortgaged;

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

    [Command(requiresAuthority = false)]
    public virtual void CmdBackFieldToBank()
    {
        RpcBackFieldToBank();
    }

    [ClientRpc]
    protected virtual void RpcBackFieldToBank()
    {
        owner = null;
        ownerCard.gameObject.SetActive(false);
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

    public override void PayByPlayer(UserFigure figure)
    {
        var payAmount = GetPayAmount();
        if (figure.userMoney < payAmount)
        {
            UserHasNoMoney();
            return;
        }
        figure.PayToUser(owner, payAmount);
    }

    protected abstract override int GetPayAmount();

    public abstract bool CanBeMortgaged();

    public void MortgageField()
    {
        owner.CmdSetUserMoney(owner.userMoney + mortgageValue);

        CmdMortgageField();
    }

    [Command(requiresAuthority = false)]
    public void CmdMortgageField()
    {
        mortgaged = true;
        RpcMortgageField();
    }

    [ClientRpc]
    public void RpcMortgageField()
    {
        ownerCard.transform.Rotate(180f, 0, 180f);
        mortgaged = true;
    }

    public void BuyBackField()
    {
        if(owner.userMoney < (mortgageValue * 1.1) )
        {
            UserHasNoMoney();
            return;
        }
        owner.CmdSetUserMoney(owner.userMoney - (int)(mortgageValue * 1.1));

        CmdBuyBackField();
    }

    [Command(requiresAuthority = false)]
    public void CmdBuyBackField()
    {
        mortgaged = false;
        RpcBuyBackField();
    }

    [ClientRpc]
    public void RpcBuyBackField()
    {
        ownerCard.transform.Rotate(-180f, 0, -180f);
        mortgaged = false;
    }

    protected IEnumerable<IFieldUnit> GetUnitsWithSameColor()
        => Field.fieldUnits.Where(unit => unit is BuyableFieldUnitBase && ((BuyableFieldUnitBase) unit).color == this.color).ToList(); 
}