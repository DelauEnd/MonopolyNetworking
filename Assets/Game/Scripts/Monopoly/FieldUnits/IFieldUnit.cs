using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public interface IFieldUnit
    {
        public GameField Field { get; }
        public List<Transform> StopPoints { get; }
        public int LockedPoints { get; set; }
        public void OnPlayerStop(UserFigure figure);
        public Vector3 GetAvailablePoint();
    }
}
