using Assets.Game.Scripts.Network.Lobby;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : NetworkBehaviour
{
    [SerializeField] public List<FieldUnitBase> fieldUnits;
    public static List<GameObject> playersOnField = new List<GameObject>();

    public override void OnStartServer()
    {
        SpawnManager.AddSpawnPoints(fieldUnits[0].stopPoints);
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
    
    public Vector3 GetAvailablePointForField(int unitNumber)
    {
        UpdateUsersOnField();
        var usersOnUnit = GetPlayersOnUnit(unitNumber);
        fieldUnits[unitNumber].lockedPoints = usersOnUnit.Count;

        return fieldUnits[unitNumber].GetAvailablePoint();
    }

    public List<GameObject> GetPlayersOnUnit(int unitNumber)
    {
        return playersOnField.Where(player => player.GetComponent<UserFigure>().currentPosition == unitNumber).ToList();
    }

    public static void UpdateUsersOnField()
    {
        playersOnField.Clear();
        playersOnField.AddRange(FindObjectsOfType<UserFigure>().Select(x=>x.transform.gameObject));
    }

    public void IsFieldBuyable()
    {

    }
}
