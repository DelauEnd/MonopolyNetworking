using Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards;
using Assets.ItemInspection;
using Assets.ItemInspection.Scripts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI
{
    public class ImproveableCardUI : AdditionalGuiBase
    {
        InspectorGui PlayerGui = null;
        ImproveableFieldUnit InspectableField = null;
        [SerializeField] GameObject ParentPanel = null;
        [SerializeField] UserFigure Figure = null;
        [SerializeField] Button[] buttons = new Button[2];

        public override void OnInspectionStart(ItemInspector inspector)
        {
            InitInspector();
            if (InspectableField.owner.hasAuthority)
            {
                ParentPanel.SetActive(true);
                buttons[0].gameObject.SetActive(true);
                buttons[1].gameObject.SetActive(true);
            }

            if (InspectableField.UserCanImproveField(Figure))
                buttons[0].interactable = true;

            if (InspectableField.UserCanWorsenField(Figure))
                buttons[1].interactable = true;
        }

        private void InitInspector()
        {
            PlayerGui = transform.GetComponentInParent<InspectorGui>();
            Figure = PlayerGui.GetComponentInParent<UserFigure>();
            InspectableField = PlayerGui.InspectableObject.GetComponent<ColoredOwnershipCard>().Field as ImproveableFieldUnit;
        }

        public override void OnInspectionStop(ItemInspector inspector)
        {
            Figure = null;
            PlayerGui = null;
            InspectableField = null;

            DisableButtons();
        }

        private void DisableButtons()
        {
            buttons[0].gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(false);
            ParentPanel.SetActive(false);
            buttons[0].interactable = false;
            buttons[1].interactable = false;
        }

        public void BuyHouseClick()
        {
            InspectableField.ImproveUnit(Figure);
        }

        public void SoldHouseClick()
        {
            InspectableField.DowngradeUnit(Figure);
        }
    }
}
