using Assets.Game.Scripts.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Assets.Game.Scripts.Monopoly.TradeBetweenUsers;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList
{
    public class OwnershipListGUI : MonoBehaviour
    {
        [SerializeField] GameObject contentPanel = null;
        [SerializeField] TMP_Text HeaderText = null;

        [SerializeField] List<GameObject> recordInstances = new List<GameObject>();

        public void ShowOwnersipList(UserFigure figure)
        {
            PreInit();

            HeaderText.text = $"{figure.UserInfo.DisplayName}'s OWNED FIELDS";

            var ownedFields = figure.OwnedFields;
            foreach (var item in ownedFields)
            {
                var recordInstance = GameObject.Instantiate(Resources.Load("Prefabs/Scrolbar/ScrolbarItem")) as GameObject;
                
                
                InitInstance(item, recordInstance);
            }
        }

        public void ShowAddableOwnersipList(UserFigure figure, UserOfferPanel userOfferPanel)
        {
            PreInit();

            HeaderText.text = $"{figure.UserInfo.DisplayName}'s TRADEABLE FIELDS";

            var ownedFields = figure.OwnedFields.Except(userOfferPanel.SelectedFields);

            ownedFields.Where(x => x.AvailableToBuy);

            foreach (var item in ownedFields)
            {
                var recordInstance = GameObject.Instantiate(Resources.Load("Prefabs/Scrolbar/TradeFieldItem")) as GameObject;

                recordInstance.GetComponent<AddableListItem>().Container = this;
                recordInstance.GetComponent<AddableListItem>().userOfferPanel = userOfferPanel;
                InitInstance(item, recordInstance);
            }
        }

        private void PreInit()
        {
            ClearInstances();
            gameObject.SetActive(true);
        }

        private void InitInstance(BuyableFieldUnitBase field, GameObject recordInstance)
        {
            recordInstance.GetComponent<FieldListItemBase>().InitOwnedField(field);
            recordInstance.transform.SetParent(contentPanel.transform);

            recordInstance.transform.localScale = Vector3.one;
            recordInstance.transform.localPosition = recordInstance.transform.position;

            recordInstances.Add(recordInstance);
        }

        public void HideOwnershipList()
        {
            ClearInstances();
            gameObject.SetActive(false);
        }

        private void ClearInstances()
        {
            recordInstances.ForEach(item => Destroy(item));
            recordInstances.Clear();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                HideOwnershipList();
        }
    }
}
