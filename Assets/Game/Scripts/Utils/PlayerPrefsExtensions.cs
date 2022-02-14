using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Utils
{
    public class PlayerPrefsExtensions
    {
        private readonly static string[] PlayerPrefsColorKeys = new string[3]
        {
            "PlayerColorR",
            "PlayerColorG",
            "PlayerColorB"
        };

        public static bool HasWritenColor
            => PlayerPrefs.HasKey(PlayerPrefsColorKeys[0]);

        public static Color GetColor()
        {
            var colorR = PlayerPrefs.GetFloat(PlayerPrefsColorKeys[0]);
            var colorG = PlayerPrefs.GetFloat(PlayerPrefsColorKeys[1]);
            var colorB = PlayerPrefs.GetFloat(PlayerPrefsColorKeys[2]);

            return new Color(colorR, colorG, colorB);
        }

        public static void WriteColor(Color color)
        {
            PlayerPrefs.SetFloat(PlayerPrefsColorKeys[0], color.r);
            PlayerPrefs.SetFloat(PlayerPrefsColorKeys[1], color.g);
            PlayerPrefs.SetFloat(PlayerPrefsColorKeys[2], color.b);
        }
    }
}
