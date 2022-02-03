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

        public override void OnStartAuthority()
        {
            virtualCamera.gameObject.SetActive(true);

            enabled = true;
        }

        [ClientCallback]
        private void OnEnable()
            => virtualCamera.enabled = true;

        [ClientCallback]
        private void OnDisable()
            => virtualCamera.enabled = false;
    }
}
