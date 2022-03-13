using Assets.Game.Scripts.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI
{
    public class EscMenu : MonoBehaviour
    {
        [SerializeField] public UserFigure Figure = null;
        [SerializeField] public GameObject EscMenuPanel = null;

        public bool showControls = false;
    }
}
