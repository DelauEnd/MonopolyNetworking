using Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards;
using Assets.ItemInspection;
using Assets.ItemInspection.Scripts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class MortgageableCardUI : AdditionalGuiBase
    {
        InspectorGui PlayerGui = null;
        BuyableFieldUnitBase InspectableField = null;
        [SerializeField] UserFigure Figure = null;

        [SerializeField] GameObject[] guiPanels = new GameObject[2];
        [SerializeField] TMP_Text[] texts = new TMP_Text[2];

        public override void OnInspectionStart(ItemInspector inspector)
        {
            Debug.Log("inspect");
            InitInspector();

            if (!InspectableField.owner.hasAuthority)
                return;

            InitPanelTexts();

            if (InspectableField.mortgaged)
            {
                guiPanels[1].gameObject.SetActive(true);
            }
            else if (InspectableField.CanBeMortgaged())
            {
                guiPanels[0].gameObject.SetActive(true);
            }
        }

        private void InitPanelTexts()
        {
            texts[0].text = ($"You can mortgage this field for <sprite index= 0>{InspectableField.mortgageValue}.\nYou can buy back field later for <sprite index= 0>{InspectableField.mortgageValue * 1.1}.");
            texts[1].text = ($"You can buy back field this for <sprite index= 0>{InspectableField.mortgageValue * 1.1}.");
        }

        private void InitInspector()
        {
            PlayerGui = transform.GetComponentInParent<InspectorGui>();
            Figure = PlayerGui.GetComponentInParent<UserFigure>();
            InspectableField = PlayerGui.InspectableObject.GetComponent<OwnershipCardBase>().Field as BuyableFieldUnitBase;
        }

        public override void OnInspectionStop(ItemInspector inspector)
        {
            Figure = null;
            PlayerGui = null;
            InspectableField = null;

            DisablePanels();
        }

        private void DisablePanels()
        {
            guiPanels[0].gameObject.SetActive(false);
            guiPanels[1].gameObject.SetActive(false);
        }

        public void MortgageField()
        {
            InspectableField.MortgageField();

            guiPanels[0].gameObject.SetActive(false);
            guiPanels[1].gameObject.SetActive(true);
        }

        public void BuyBackField()
        {
            InspectableField.BuyBackField();

            guiPanels[0].gameObject.SetActive(true);
            guiPanels[1].gameObject.SetActive(false);
        }
    }
}
