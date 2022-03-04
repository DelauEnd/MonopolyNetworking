using Assets.ItemInspection.Scripts.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.ItemInspection.Scripts
{
    public class InspectionByClick : InspectorHandlerBase
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (Inspection.gameObject.FindComponents<Collider>().Any(col => col == hit.collider) && Inspection.CanInspect)
                        Inspection.SetEnableInspecting(true);
                }
            }
        }
    }
}
