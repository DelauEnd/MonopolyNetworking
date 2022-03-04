using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class InspectorUtils
    {
        public static UserFigure GetUserFigure(GameObject additionCanvas)
            => additionCanvas.transform.parent.GetComponentInParent<UserFigure>();
    }
}
