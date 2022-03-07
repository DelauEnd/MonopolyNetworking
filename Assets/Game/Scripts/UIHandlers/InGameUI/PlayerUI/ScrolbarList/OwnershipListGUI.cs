using Assets.Game.Scripts.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList
{
    public class OwnershipListGUI : MonoBehaviour
    {
        [SerializeField] GameObject contentPanel = null;
        [SerializeField] TMP_Text HeaderText = null;

        [SerializeField] List<GameObject> recordInstances = new List<GameObject>();

        public void ShowOwnersipList(UserFigure figure)
        {
            ClearInstances();

            gameObject.SetActive(true);

            HeaderText.text = $"{figure.UserInfo.DisplayName}'s OWNED FIELDS";

            var ownedFields = figure.OwnedFields;
            foreach (var item in ownedFields)
            {
                var recordInstance = GameObject.Instantiate(Resources.Load("Prefabs/Scrolbar/ScrolbarItem")) as GameObject;
                recordInstance.GetComponent<OwnershipListItem>().InitOwnedField(item);
                recordInstance.transform.SetParent(contentPanel.transform);

                recordInstance.transform.localScale = Vector3.one;
                recordInstance.transform.localPosition = recordInstance.transform.position;

                recordInstances.Add(recordInstance);
            }
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
