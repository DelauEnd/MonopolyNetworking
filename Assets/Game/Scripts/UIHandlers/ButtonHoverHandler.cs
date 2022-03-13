using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.UIHandlers
{
    [RequireComponent(typeof(EventTrigger))]
    public class ButtonHoverHandler : MonoBehaviour
    {
        [SerializeField] Text TextToHandle = null;

        Font HoverText = null;
        Font UnhoverText = null;
        EventTrigger ButtonEvents = null;

        int baseFontSize = 0;

        private void Awake()
        {
            baseFontSize = TextToHandle.fontSize;

            HoverText = Resources.Load("Fonts/Monopoly-Dotted") as Font;
            UnhoverText = TextToHandle.font;

            ButtonEvents = GetComponent<EventTrigger>();
            AddUnhoverTrigger();
            AddHoverTrigger();
        }

        private void AddUnhoverTrigger()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((data) => { UnhoverButton((PointerEventData)data); });
            ButtonEvents.triggers.Add(entry);
        }
        
        private void AddHoverTrigger()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { HoverButton((PointerEventData)data); });
            ButtonEvents.triggers.Add(entry);
        }

        private void HoverButton(PointerEventData data)
        {
            if (GetComponent<Button>().interactable)
            {
                TextToHandle.font = HoverText;
                TextToHandle.fontSize = baseFontSize - 4;
            }
        }

        private void UnhoverButton(PointerEventData data)
        {
            TextToHandle.font = UnhoverText;
            TextToHandle.fontSize = baseFontSize;
        }
    }
}
