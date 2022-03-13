using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.UIHandlers.InGameUI.PlayerUI.ScrolbarList
{
    public abstract class FieldListItemBase : MonoBehaviour
    {
        protected BuyableFieldUnitBase UnitField;

        public abstract void InitOwnedField(BuyableFieldUnitBase fieldUnit);
    }
}
