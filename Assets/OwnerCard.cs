using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Game.Scripts.Network.Lobby;

public class OwnerCard : MonoBehaviour
{
    [SerializeField]
    [ColorUsage(false)]
    private Color fieldColor;
    [SerializeField] private Image unitColorImg = null;

    [SerializeField] TMP_Text ownerName = null;
    [SerializeField] RawImage ownerColorImage = null;

    private void Awake()
    {
        unitColorImg.color = fieldColor;
    }

    public void SetOwnerInfo(NetworkGamePlayerLobby newOwner)
    {
        ownerName.text = newOwner.DisplayName;
        ownerColorImage.color = newOwner.DisplayColor;
    }
}
