using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.Notification
{
    public class NotificationHandler : MonoBehaviour
    {
        public GameObject[] basePanel = null;

        public NotificationUI InstantiateNotification(int basePanelInd = 0)
        {
            var notificationInstance = GameObject.Instantiate(Resources.Load("Prefabs/Notification")) as GameObject;

            notificationInstance.transform.SetParent(basePanel[basePanelInd].transform);

            notificationInstance.transform.localScale = Vector3.one;
            notificationInstance.transform.localPosition = notificationInstance.transform.position;

            return notificationInstance.GetComponent<NotificationUI>();
        }
    }
}
