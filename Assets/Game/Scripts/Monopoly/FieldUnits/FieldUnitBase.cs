using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldUnitBase : NetworkBehaviour
{
    public GameField gameField = null;

    public List<Transform> stopPoints;

    public UserFigure owner = null;
    public int initialCost = 100;

    public int lockedPoints = 0;

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
}