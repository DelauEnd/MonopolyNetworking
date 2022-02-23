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
        public KeyCode closeKey;

        private void Update()
        {
            if(Input.GetKeyDown(closeKey))
                Inspection.SetEnableInspecting(false);
        }

    }
}
