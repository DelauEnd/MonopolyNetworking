using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection
{
    public interface IChangeInspectorState
    {
        public void OnInspectionStart(GameObject inspectedObject);
        public void OnInspectionStop(GameObject inspectedObject);
    }
}
