using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;
using System;
using Assets.Game.Scripts.Network.Lobby;
using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.ItemInspection.Scripts.Utils;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class PlayerUIHandler : NetworkBehaviour
    {
        public Canvas UserUI = null;
        [HideInInspector] public PlayerInfoUI PlayerInfoUI = null;
        [HideInInspector] public GameUnitsPlayerUI GameUnitsPlayerUI = null;

        private void Awake()
        {
            PlayerInfoUI = UserUI.GetComponent<PlayerInfoUI>();
            GameUnitsPlayerUI = UserUI.GetComponent<GameUnitsPlayerUI>();
        }

        public override void OnStartAuthority()
        {
            UserUI.gameObject.SetActive(true);

            var camera = FindObjectOfType<InspectorCamera>().GetComponent<Camera>();
            UserUI.worldCamera = camera;

            this.enabled = true;    
        }

        [ClientCallback]
        private void OnEnable()
        {
            UserUI.enabled = true;
        }

        [ClientCallback]
        private void OnDisable()
        {
            UserUI.enabled = false;
        }

    }
}
