using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection.Scripts
{
    public abstract class ImmortalInspectorHandlerBase : MonoBehaviour
    {
        public ItemInspector Inspection { get; private set;}
        public abstract bool OverrideScale { get; }
        protected virtual void Awake()
        {
            Inspection = GetComponent<ItemInspector>();
        }
    }
}
