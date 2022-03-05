using UnityEngine;

namespace Assets.ItemInspection.Scripts
{
    public class InspectableDimensions : ImmortalInspectorHandlerBase
    {
        [Header("Inspection settings")]
        public Vector3 Scale = Vector3.one;

        public override bool OverrideScale => true;

        protected override void Awake()
        {
            base.Awake();
            if (Inspection.inspectableInstance == null)
                return;
            Inspection.inspectableInstance.transform.localScale = Scale;
        }
    }
}
