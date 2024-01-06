using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.Notification;
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
    public void CmdChangeOwner(UserFigure newOwner)
    {
        RpcChangeOwner(newOwner);
    } 
    
    [ClientRpc]
    public void RpcChangeOwner(UserFigure newOwner)
    {
        owner = newOwner;
        if (ownerCard != null)
        {
            ownerCard.SetVisible(true);
            ownerCard.SetNewOwner(newOwner);
        }
    }    

    [Command(requiresAuthority = false)]
    public void CmdBuyCurrentField(UserFigure figure)
    {
        RpcBuyCurrentField(figure);       
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
    private void RpcBuyCurrentField(UserFigure newOwnerFigure)
    {
        if (newOwnerFigure.userMoney < unitPrice)
        {
            NotEnoughtMoneyToBuy(newOwnerFigure, unitPrice, new Action(() => { OnPlayerStop(newOwnerFigure); }));
            return;
        }

        owner = newOwnerFigure;

        if (ownerCard != null)
        {
            ownerCard.SetVisible(true);
            ownerCard.SetNewOwner(newOwnerFigure);
        }

        newOwnerFigure.CmdSetUserMoney(newOwnerFigure.userMoney - unitPrice);
        newOwnerFigure.UIHandler.GameUnitsPlayerUI.EndTurn(); ;
        newOwnerFigure.UIHandler.GameUnitsPlayerUI.payIfStayUnitUI.HideUI(); ;
    }



    protected void NotEnoughtMoneyToBuy(UserFigure figure, int requiredMoney, Action action)
    {
        var notif = figure.gameObject.GetComponent<NotificationHandler>().InstantiateNotification(1);
        notif.GetComponent<RectTransform>().sizeDelta = new Vector2(550, 300);
        notif.ShowOneButtonNotification("You have not enought money to pay. You can offer exchange to other players or mortgate your fields", "confirm", action);
    }

    public override bool PayByPlayer(UserFigure figure)
    {
        var payAmount = GetPayAmount();
        if (figure.userMoney < payAmount)
        {
            UserHasNoMoney(figure, payAmount, new Action(() => { PayByPlayer(figure); }));
            return false;
        }
        figure.PayToUser(owner, payAmount);
        return true;
    }

    protected abstract override int GetPayAmount();

    public virtual bool CanBeMortgaged()
        => !mortgaged;

    public virtual bool CanBeTransfered()
    {
        return true;
    }    

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
            UserHasNoMoney(owner, (int)(mortgageValue * 1.1), new Action(() => { BuyBackField(); }));
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
        => Field.fieldUnits.OfType<BuyableFieldUnitBase>().Where(unit =>  unit.color == this.color).ToList(); 
}