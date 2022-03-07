using Assets.Game.Scripts.Utils.Extensions;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.BaseUnit
{
    public abstract class DrawCardFieldBase : FieldUnitBase
    {
        public List<DrawableCardBase> DrawableCards = null;

        public DrawableCardBase GetRandomCard()
            => DrawableCards.ToList()[new System.Random().Next(0, DrawableCards.Count())];

        public void ShowCard(DrawableCardBase card)
        {
            var cardInd = DrawableCards.FindIndex(cardToFind =>cardToFind.name == card.name);
            DrawableCards.Where(card => card.gameObject.activeSelf).ForEach(card => card.gameObject.SetActive(false));
            card.gameObject.SetActive(true);
            CmdShowCard(cardInd);
        }

        [Command(requiresAuthority = false)]
        private void CmdShowCard(int cardInd)
        {
            DrawableCards.Where(card => card.gameObject.activeSelf).ForEach(card => card.gameObject.SetActive(false));
            DrawableCards[cardInd].gameObject.SetActive(true);

            RpcShowCard(cardInd);
        }

        [ClientRpc]
        private void RpcShowCard(int cardInd)
        {
            DrawableCards.Where(card => card.gameObject.activeSelf).ForEach(card => card.gameObject.SetActive(false));
            DrawableCards[cardInd].gameObject.SetActive(true);
        }
    }
}
