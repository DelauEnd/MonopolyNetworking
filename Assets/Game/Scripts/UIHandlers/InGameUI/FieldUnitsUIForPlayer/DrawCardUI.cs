﻿using Assets.Game.Scripts.Controls;
using Assets.Game.Scripts.Monopoly.Enums;
using Assets.Game.Scripts.Monopoly.FieldUnits;
using Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit;
using Assets.ItemInspection;
using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.FieldUnitsUIForPlayer
{
    public class DrawCardUI : UnitUIBase
    {
        [SerializeField] Button DrawCardButton = null;
        [SerializeField] Button ConfirmButton = null;
        DrawableCardBase DrawableCard { get; set; }


        public override void BuildMessage(string message, string drawCardButtonText = null, string confirmButtonText = null)
        {
            MessageText.text = message;

            if (drawCardButtonText != null)
                DrawCardButton.GetComponentInChildren<Text>().text = drawCardButtonText;

            if (confirmButtonText != null)
                ConfirmButton.GetComponentInChildren<Text>().text = confirmButtonText;
        }

        public void DrawCard()
        {
            DrawableCard = ((DrawCardFieldBase)Figure.GetCurrentUnit()).GetRandomCard();
            Debug.Log($"Drawed card {DrawableCard.name}");
            ((DrawCardFieldBase)Figure.GetCurrentUnit()).ShowCard(DrawableCard);

            ConfirmButton.gameObject.SetActive(true);
            DrawCardButton.gameObject.SetActive(false);
            MessageText.text = string.Empty;

            DrawableCard.transform.parent.GetComponent<ItemInspector>().SetEnableInspecting(true);
        }

        public void ConfirmCard()
        {
            DrawableCard.transform.parent.GetComponent<ItemInspector>().SetEnableInspecting(false);
            Debug.Log($"Using card {DrawableCard.name}");
            DrawableCard.OnUserDrawCard(Figure);
            HideUI();

            if (DrawableCard.CardType != DrawableCardType.MovePlayer)
                EndTurn();
            else
                FindObjectsOfType<PlayerCameraController>().FirstOrDefault(controller => controller.hasAuthority).virtualCamera.gameObject.SetActive(true);

        }

        public override void HideUI()
        {
            gameObject.SetActive(false);
        }

        public override void ShowUI()
        {
            gameObject.SetActive(true);

            DrawCardButton.gameObject.SetActive(true);
            ConfirmButton.gameObject.SetActive(false);
        }
    }
}
