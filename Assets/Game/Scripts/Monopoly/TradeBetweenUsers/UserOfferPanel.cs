using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList;

namespace Assets.Game.Scripts.Monopoly.TradeBetweenUsers
{
    public class UserOfferPanel : MonoBehaviour
    {
        [SerializeField] GameObject contentPanel = null;
        [SerializeField] Slider MoneySlide = null;

        [SerializeField] TMP_Text MaxMoney = null;
        [SerializeField] TMP_InputField MoneyText = null;

        [SerializeField] public List<BuyableFieldUnitBase> SelectedFields = new List<BuyableFieldUnitBase>();
        [SerializeField] List<GameObject> selectedInstances = new List<GameObject>();

        UserFigure player = null;
        [SerializeField] OwnershipListGUI tabMenu = null;

        public void InitOfferPanel(UserFigure figure)
        {
            player = figure;
            MoneySlide.maxValue = figure.userMoney;
            MaxMoney.text = figure.userMoney.ToString();
            MoneyText.text = "0";

            player.OnUserMoneyChanged += UpdateMoney;
        }

        private void UpdateMoney(int newMoneyAmount)
        {
            MoneySlide.maxValue = newMoneyAmount;
            MaxMoney.text = newMoneyAmount.ToString();

            if (int.Parse(MoneyText.text) > newMoneyAmount)
                MoneyText.text = newMoneyAmount.ToString();
        }

        public void ChangeMoneyText()
        {
            if (player == null)
                return;

            var newMoneyValue = int.Parse(MoneyText.text);
            if (newMoneyValue > player.userMoney)
                newMoneyValue = player.userMoney;

            MoneyText.text = newMoneyValue.ToString();
            MoneySlide.value = newMoneyValue;
        }
        
        public void ChangeMoneySlide()
        {
            if (player == null)
                return;

            var newMoneyValue = (int)MoneySlide.value;

            MoneyText.text = newMoneyValue.ToString();
        }

        private void ValidateUserMoney()
        {
            if (MoneySlide.maxValue != player.userMoney)
            {
                MoneySlide.maxValue = player.userMoney;
                MaxMoney.text = player.userMoney.ToString();
            }
        }

        public TradeOffer BuildOffer()
            => new TradeOffer
            {
                MoneyAmount = (int)MoneySlide.value,
                FieldUnitIndexes = GetSelectedFieldsIndexes()
            };

        public int[] GetSelectedFieldsIndexes()
        {
            int[] indexes = new int[SelectedFields.Count];

            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i] = player.Field.fieldUnits.FindIndex(field => SelectedFields[i]);
            }

            return indexes;
        }

        public void ClearOffer()
        {
            if (player != null)
            {
                player.OnUserMoneyChanged -= UpdateMoney;
                player = null;
            }
            MoneySlide.value = 0;
            MoneyText.text = "0";
            SelectedFields.Clear();
            selectedInstances.ForEach(instance=> Destroy(instance));
            selectedInstances.Clear();
        }

        public void AddFieldToSelected(BuyableFieldUnitBase field)
        {
            if (field.owner != player)
                return;

            SelectedFields.Add(field);

            var recordInstance = GameObject.Instantiate(Resources.Load("Prefabs/Scrolbar/ScrolbarItem")) as GameObject;

            recordInstance.GetComponent<OwnershipListItem>().InitOwnedField(field);
            InitInstance(field, recordInstance);
        }

        private void InitInstance(BuyableFieldUnitBase field, GameObject recordInstance)
        {

            recordInstance.transform.SetParent(contentPanel.transform);

            recordInstance.transform.localScale = Vector3.one;
            recordInstance.transform.localPosition = recordInstance.transform.position;

            selectedInstances.Add(recordInstance);
        }

        public void ShowUserFields()
        {
            tabMenu.ShowAddableOwnersipList(player, this);
        }
    }
}