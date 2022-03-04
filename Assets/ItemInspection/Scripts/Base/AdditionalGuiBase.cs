using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection.Scripts.Base
{
    public abstract class AdditionalGuiBase : MonoBehaviour
    {
        public abstract void OnInspectionStart(ItemInspector inspector);
        public abstract void OnInspectionStop(ItemInspector inspector);
    }
}
