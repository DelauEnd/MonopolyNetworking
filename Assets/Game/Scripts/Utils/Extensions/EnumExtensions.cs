using Assets.Game.Scripts.Monopoly.FieldUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static Color GetColor(this UnitColor color)
        {
            switch (color)
            {
                case UnitColor.Red:
                    return new Color(0.98f, 0.27f, 0.31f);
                case UnitColor.Yellow:
                    return new Color(1f, 1f, 0.17f);
                case UnitColor.Green:
                    return new Color(0.25f, 0.81f, 0.52f);
                case UnitColor.Blue:
                    return new Color(0.15f, 0.58f, 0.89f);
                case UnitColor.Brown:
                    return new Color(0.67f, 0.47f, 0.38f);
                case UnitColor.White:
                    return new Color(0.78f, 0.98f, 1f);
                case UnitColor.Purple:
                    return new Color(0.91f, 0.38f, 0.75f);
                case UnitColor.Orange:
                    return new Color(1f, 0.71f, 0.29f);
                default:
                    return Color.clear;
            }
        }
    }
}
