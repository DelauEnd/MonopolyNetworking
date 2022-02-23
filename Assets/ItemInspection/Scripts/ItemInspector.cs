using Assets.ItemInspection.Scripts;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection
{
    public class ItemInspector : MonoBehaviour
    {
        private InspectorGui InspectorGui;
        [HideInInspector] public bool isInspecting;
        [SerializeField] public GameObject AdditionalInspectingGui = null;

        private bool inited = false;
        [HideInInspector] public bool CanInspect
            => !InspectorGui.guiEnabled;

        private GameObject inspectableInstance = null;
        private GameObject addGuiInstance = null;

        IChangeInspectorState inspectorState;

        private void Start()
        {
            InitAddInspectorState();
        }

        private void InitInspector()
        {
            InitMainGui();
        }

        private void InitAddInspectorState()
        {
            var addInspectorState = GetComponent<IChangeInspectorState>();
            if (addInspectorState != null)
                inspectorState = addInspectorState;
        }

        private void InitMainGui()
        {
            var mainGui = FindObjectsOfType<InspectorGui>().FirstOrDefault(x=>x.gameObject.activeSelf);
            if (mainGui != null)
            {
                InspectorGui = mainGui;
                inited = true;
            }
        }

        private void InstantiateMainGui()
        {
            var guiInstance = GameObject.Instantiate(Resources.Load("Prefabs/InspectorGui")) as GameObject;           
            InspectorGui = guiInstance.GetComponent<InspectorGui>();
            InspectorGui.SetEnabledGui(false);
        }

        private void InitAdditionalGui()
        {
            if (AdditionalInspectingGui != null)
            {
                addGuiInstance = GameObject.Instantiate(this.AdditionalInspectingGui.gameObject);

                InspectorGui.SetAdditionalGui(addGuiInstance);
                addGuiInstance.SetActive(false);
            }
        }

        private void InitInspectedItem()
        {
            inspectableInstance = GameObject.Instantiate(this.transform.gameObject);

            DestroyInstanceInspector();

            InspectorGui.SetItemToInspect(inspectableInstance);
        }

        private void DestroyInstanceInspector()
        {
            foreach (var comp in inspectableInstance.GetComponents<InspectorHandlerBase>())
                Destroy(comp);
            Destroy(inspectableInstance.GetComponent<ItemInspector>());
        }

        public void SetEnableInspecting(bool enable)
        {
            if (enable)
                EnableInspecting();
            else
                DisableInspecting();
        }

        private void DisableInspecting()
        {
            isInspecting = false;

            Destroy(addGuiInstance);
            Destroy(inspectableInstance);
            InspectorGui.SetEnabledGui(false);

            if (inspectorState != null)
                inspectorState.OnInspectionStop(this.gameObject);

            if (AdditionalInspectingGui != null)
                AdditionalInspectingGui.gameObject.SetActive(false);
        }

        private void EnableInspecting()
        {
            isInspecting = true;

            InitAdditionalGui();
            InitInspectedItem();
            InspectorGui.SetEnabledGui(true);

            if (inspectorState!=null)
                inspectorState.OnInspectionStart(this.gameObject);

            if (AdditionalInspectingGui != null)
                AdditionalInspectingGui.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!inited)
                InitMainGui();
        }
    }
}
