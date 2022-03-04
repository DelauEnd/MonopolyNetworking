using Assets.ItemInspection.Scripts.Utils;
using Assets.ItemInspection.Scripts.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection
{
    public class InspectorGui : MonoBehaviour
    {
        public GameObject InspectableObject = null;

        [SerializeField] private Canvas BaseInspectingCanvas;
        [SerializeField] private GameObject ItemToInspect;

        private Vector3 MinBound = new Vector3(2f, 1f, 0f);
        private Vector3 MaxBound = new Vector3(-2f, -1f, 0f);

        public bool guiEnabled
            => BaseInspectingCanvas.gameObject.activeSelf;

        private void Awake()
        {
            InitCamera();
        }

        private void InitCamera()
        {
            var camera = FindObjectOfType<InspectorCamera>().GetComponent<Camera>();
            if (camera != null)
                SetGuiCamera(camera);
        }

        public void SetEnabledGui(bool enabled)
        {
            BaseInspectingCanvas.gameObject.SetActive(enabled);
        }

        public void SetAdditionalGui(GameObject addGui)
        {
            addGui.transform.SetParent(this.transform);
            addGui.GetComponent<Canvas>().worldCamera = BaseInspectingCanvas.worldCamera;
            addGui.transform.localPosition = Vector3.zero;
        }

        public void SetItemToInspect(GameObject item)
        {
            item.transform.localPosition = ItemToInspect.transform.position;
            item.transform.rotation = ItemToInspect.transform.localRotation;
            SetupInspectableItemScale(item);

            item.transform.SetParent(ItemToInspect.transform);

            item.gameObject.ChangeObjectLayer(LayerMask.NameToLayer("UI"));
        }

        private void SetupInspectableItemScale(GameObject item)
        {
            var bounds = MeshAdditions.GetRenderBounds(item);

            var bound1 = bounds.min;
            var bound2 = bounds.max;

            float[] mults = new float[4];

            mults[0] = MinBound.x * bound1.x;
            mults[1] = MinBound.y * bound1.y;
            mults[2] = MaxBound.x * bound2.x;
            mults[3] = MaxBound.y * bound2.y;

            var multiplier = mults.Max(x => x)/2.5f;

            item.transform.localScale /= Math.Abs(multiplier);

        }

        public void SetGuiCamera(Camera camera)
        {
            if (BaseInspectingCanvas.worldCamera == null)
                BaseInspectingCanvas.worldCamera = camera;
        }
    }
}
