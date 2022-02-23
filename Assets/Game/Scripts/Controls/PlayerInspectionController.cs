using Assets.ItemInspection;
using Assets.ItemInspection.Scripts.Utils.Extensions;
using Cinemachine;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Game.Scripts.Controls
{
    public class PlayerInspectionController : NetworkBehaviour
    {
        [SerializeField] InspectorGui inspectorGui = null;

        public override void OnStartAuthority()
        {
            inspectorGui.gameObject.SetActive(true);
        }
    }
}
