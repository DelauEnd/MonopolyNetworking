using Assets.ItemInspection.Scripts;
using Assets.ItemInspection.Scripts.Base;
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
        public InspectorGui InspectorGui;
        [HideInInspector] public bool isInspecting;
        [SerializeField] public AdditionalGuiBase AdditionalInspectingGui = null;

        private bool inited = false;
        [HideInInspector] public bool CanInspect
            => !InspectorGui.guiEnabled;

        public GameObject inspectableInstance = null;
        public GameObject addGuiInstance = null;

        private void InitMainGui()
        {
            var mainGui = FindObjectsOfType<InspectorGui>().FirstOrDefault(x=>x.gameObject.activeSelf);
            if (mainGui != null)
            {
                InspectorGui = mainGui;
                inited = true;
            }
        }

        [Obsolete]
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
            InspectorGui.InspectableObject = null;

            if(addGuiInstance != null)
                addGuiInstance.GetComponent<AdditionalGuiBase>().OnInspectionStop(this);
          
            Destroy(addGuiInstance);
            Destroy(inspectableInstance);
            InspectorGui.SetEnabledGui(false);
       
            if (addGuiInstance != null)
                addGuiInstance.gameObject.SetActive(false);
        }

        private void EnableInspecting()
        {
            isInspecting = true;
            InspectorGui.InspectableObject = this.gameObject;

            InitAdditionalGui();
            InitInspectedItem();

            if (addGuiInstance != null)
                addGuiInstance.GetComponent<AdditionalGuiBase>().OnInspectionStart(this);

            InspectorGui.SetEnabledGui(true);

            if (addGuiInstance != null)
                addGuiInstance.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!inited)
                InitMainGui();
        }
    }
}
