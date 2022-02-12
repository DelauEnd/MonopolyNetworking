using Cinemachine;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Controls
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CinemachineFreeLook virtualCamera = null;

        [Header("UI")]
        [SerializeField] public Canvas userUI = null;

        public override void OnStartAuthority()
        {
            virtualCamera.gameObject.SetActive(true);
            userUI.gameObject.SetActive(true);

            var camera = FindObjectOfType<Camera>();
            userUI.worldCamera = camera;

            enabled = true;
        }

        [ClientCallback]
        private void OnEnable()
        { 
            virtualCamera.enabled = true;
            userUI.enabled = true;
        }

        [ClientCallback]
        private void OnDisable()
        {
            virtualCamera.enabled = false;
            userUI.enabled = false;
        }
    }
}
