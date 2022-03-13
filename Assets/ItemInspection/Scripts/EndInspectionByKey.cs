using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection.Scripts
{
    public class EndInspectionByKey : InspectorHandlerBase
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Inspection.SetEnableInspecting(false);
            }
        }
    }
}
