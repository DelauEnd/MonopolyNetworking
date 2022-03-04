using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public class UnitBuildings : MonoBehaviour
    {
        [SerializeField] List<GameObject> buildings = null;

        private void Awake()
        {
            DisableAll();
        }

        public void UpdateBuildings(int unitImprovementLevel)
        {
            if (unitImprovementLevel < 5)
                DrawHouses(unitImprovementLevel);
            else
                DrawHotel();
        }

        private void DrawHotel()
        {
            DisableAll();

            buildings[4].gameObject.SetActive(true);
        }

        private void DrawHouses(int unitImprovementLevel)
        {
            DisableAll();

            for (int i = 0; i < unitImprovementLevel; i++)
            {
                buildings[i].gameObject.SetActive(true);
            }
        }

        private void DisableAll()
        {
            foreach (var build in buildings)
                build.gameObject.SetActive(false);
        }
    }
}
