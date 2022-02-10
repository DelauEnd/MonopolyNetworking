using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldUnit : MonoBehaviour
{
    public GameField gameField = null;

    public List<Transform> stopPoints;

    public int lockedPoints = 0;

    private void Awake()
    {
        InitPoint();
    }

    //public override void OnStartClient()
    //{
    //    InitPoint();
    //}

    //public override void OnStartServer()
    //{
    //    InitPoint();
    //}

    private void InitPoint()
    {
        gameField = GetComponentInParent<GameField>();
        stopPoints = new List<Transform>(GetComponentsInChildren<Transform>()).Where(obj => obj != this.transform).ToList();
    }

    public Vector3 GetAvailablePoint()
        => stopPoints[lockedPoints].position;
}