using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits.OwnershipCards
{
    public abstract class OwnershipCardBase : MonoBehaviour
    {
        public FieldUnitBase Field { get; set; }
        public abstract void InitCard(BuyableFieldUnitBase fieldUnit);
        public abstract void SetVisible(bool visible);
    }
}
