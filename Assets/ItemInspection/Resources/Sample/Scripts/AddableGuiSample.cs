using Assets.ItemInspection;
using Assets.ItemInspection.Scripts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class AddableGuiSample : AdditionalGuiBase
    {
        public override void OnInspectionStart(ItemInspector inspector)
        {
            var gui = transform.GetComponentInParent<InspectorGui>();

            Debug.Log(gui.name);
            Debug.Log($"Inspection {gui.InspectableObject} ON");
        }

        public override void OnInspectionStop(ItemInspector inspector)
        {
            var gui = transform.GetComponentInParent<InspectorGui>();

            Debug.Log(gui.name);
            Debug.Log($"Inspection {gui.InspectableObject} OFF");
        }
    }
}
