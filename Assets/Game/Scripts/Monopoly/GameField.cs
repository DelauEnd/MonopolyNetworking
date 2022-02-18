using Assets.Game.Scripts.Monopoly.FieldUnits;
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
        SpawnManager.AddSpawnPoints(fieldUnits[0].StopPoints);
    }

    private void OnDestroy()
        => SpawnManager.ClearSpawnPoint();
    
    public Vector3 GetAvailablePointForField(int unitNumber)
    {
        UpdateUsersOnField();
        var usersOnUnit = GetPlayersOnUnit(unitNumber);
        fieldUnits[unitNumber].LockedPoints = usersOnUnit.Count;

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
}
