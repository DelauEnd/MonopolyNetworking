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
        [SerializeField] public List<AdditionalGuiBase> AdditionalInspectingGuis = null;

        private bool inited = false;
        [HideInInspector]
        public bool CanInspect
            => !InspectorGui.guiEnabled;
        [Header("Instances")]
        public GameObject inspectableInstance = null;
        public List<GameObject> addGuiInstances = new List<GameObject>();

        private void InitMainGui()
        {
            var mainGui = FindObjectsOfType<InspectorGui>().FirstOrDefault(x => x.gameObject.activeSelf);
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
            foreach (var gui in AdditionalInspectingGuis)
            {
                var instance = GameObject.Instantiate(gui.gameObject);
                InspectorGui.SetAdditionalGui(instance);


                addGuiInstances.Add(instance);
                instance.SetActive(true);
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

             foreach (var gui in addGuiInstances)
             {
                gui.GetComponent<AdditionalGuiBase>().OnInspectionStop(this);
             }

            foreach (var gui in addGuiInstances)
            {
                Destroy(gui);
            }
            addGuiInstances.Clear();

            Destroy(inspectableInstance);
            InspectorGui.SetEnabledGui(false);

            
            foreach (var gui in addGuiInstances)
            {
                    gui.SetActive(false);
            }
        }

        private void EnableInspecting()
        {
            isInspecting = true;
            InspectorGui.InspectableObject = this.gameObject;

            InitAdditionalGui();
            InitInspectedItem();

            foreach (var gui in addGuiInstances)
            {
                gui.GetComponent<AdditionalGuiBase>().OnInspectionStart(this);
            }

            InspectorGui.SetEnabledGui(true);


            foreach (var gui in addGuiInstances)
            {
                gui.SetActive(true);
            }
        }

        private void Update()
        {
            if (!inited)
                InitMainGui();
        }
    }
}
