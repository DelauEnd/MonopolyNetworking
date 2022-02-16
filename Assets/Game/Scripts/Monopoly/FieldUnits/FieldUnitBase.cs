using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldUnitBase : NetworkBehaviour
{
    [Header("Unit info")]
    public GameField gameField = null;
    public List<Transform> stopPoints;
    public int lockedPoints = 0;
    public UnitColor color;
    public OwnerCard ownerCard;

    [Header("Interact info")]
    public bool buyable;
    public UserFigure owner = null;
    public int maxImproveLevel;
    public int currentImproveLevel;
    public int[] improvePrices;
    public int[] rentalPrices;

    private void Awake()
    {
        InitPoint();
    }

    private void InitPoint()
    {
        gameField = GetComponentInParent<GameField>();
        stopPoints = new List<Transform>(GetComponentsInChildren<Transform>()).Where(obj => obj != this.transform).ToList();
    }

    public Vector3 GetAvailablePoint()
        => stopPoints[lockedPoints].position;

    public bool IsBuyableByUser()
        => (buyable == true && owner == null );

    public bool IsImproveableForUser(UserFigure figure)
        => (owner == figure && UserOwnAllColoredUnits(figure) && currentImproveLevel < maxImproveLevel);

    public void BuyByUser(UserFigure figure)
    {
        if (figure.userMoney < improvePrices[0])
            UserHasNoMoney();
        figure.RpcSetUserMoney(figure.userMoney - improvePrices[0]);
        owner = figure;
    }

    public void ChangeOwner(NetworkGamePlayerLobby newOwner)
    {
        ownerCard.SetOwnerInfo(newOwner);
        ownerCard.gameObject.SetActive(true);
    }

    private void UserHasNoMoney()
    {
        //User has no money logic
        throw new NotImplementedException();
    }

    public bool UserOwnAllColoredUnits(UserFigure figure)
        => gameField.fieldUnits.Where(unit => unit.color == color).All(unit => unit.owner == figure);
}