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
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class PlayerUIHandler : NetworkBehaviour
    {
        public Canvas UserUI = null;
        [Header("UI")]
        public PlayerInfoUI PlayerInfoUI = null;
        public GameUnitsPlayerUI GameUnitsPlayerUI = null;
        public TabMenuUI TabMenuUI = null;
        public EscMenu EscMenu = null;

        private void Awake()
        {
            PlayerInfoUI = UserUI.GetComponent<PlayerInfoUI>();
            GameUnitsPlayerUI = UserUI.GetComponent<GameUnitsPlayerUI>();
        }

        public override void OnStartAuthority()
        {
            UserUI.gameObject.SetActive(true);
            TabMenuUI.transform.parent.GetComponent<Canvas>().gameObject.SetActive(true);

            var camera = FindObjectOfType<InspectorCamera>().GetComponent<Camera>();

            UserUI.worldCamera = camera;
            TabMenuUI.transform.parent.GetComponent<Canvas>().worldCamera = camera;

            this.enabled = true;    
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (TabMenuUI.ownershipGUI.gameObject.activeSelf)
                {
                    TabMenuUI.ownershipGUI.HideOwnershipList();
                }
                else
                {
                    EscMenu.showControls = !EscMenu.showControls;
                    EscMenu.EscMenuPanel.gameObject.SetActive(EscMenu.showControls);
                }
            }
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
