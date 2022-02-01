using Assets.Game.Scripts.Network.Lobby;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : MonoBehaviour
{
    Transform[] gameObjects;
    public List<FieldUnit> fieldUnits = new List<FieldUnit>();

    private void Awake()
    {
        FillNodes();

        SpawnManager.AddSpawnPoints(fieldUnits[0].stopPoints);
        fieldUnits[0].lockedPoints = SpawnManager.usedPoints;
    }

    private void OnDestroy()
        => SpawnManager.ClearSpawnPoint();

    /// <summary>
    /// Draws game field route
    /// </summary>
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    for (int i = 0; i < fieldUnits.Count; i++)
    //    {
    //        var curUnitPos = fieldUnits[i].GetAvailablePoint();

    //        if (i > 0)
    //        {
    //            var prevUnitPos = fieldUnits[i - 1].GetAvailablePoint();
    //            Gizmos.DrawLine(prevUnitPos, curUnitPos);
    //        }
    //    }
    //}

    /// <summary>
    /// Add field units to unit list
    /// </summary>
    private void FillNodes()
    {
        fieldUnits = new List<FieldUnit>(GetComponentsInChildren<FieldUnit>());
    }
}
