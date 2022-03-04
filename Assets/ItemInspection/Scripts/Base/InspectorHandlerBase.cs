using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection.Scripts
{
    [RequireComponent(typeof(ItemInspector))]
    public abstract class InspectorHandlerBase : MonoBehaviour
    {
        public ItemInspector Inspection { get; private set;}

        protected virtual void Awake()
        {
            Inspection = GetComponent<ItemInspector>();
        }
    }
}
