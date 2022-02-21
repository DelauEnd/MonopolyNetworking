using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Monopoly.FieldUnits
{
    public abstract class FieldUnitBase : NetworkBehaviour, IFieldUnit
    {
        public GameField Field { get; private set; }

        public List<Transform> StopPoints { get; private set; }

        public int LockedPoints { get; set; }

        protected virtual void Awake()
        {
            Field = GetComponentInParent<GameField>();
            StopPoints = new List<Transform>(GetComponentsInChildren<Transform>()).Where(obj => obj != this.transform).ToList();
        }

        abstract public void OnPlayerStop(UserFigure figure);

        public Vector3 GetAvailablePoint()
            => StopPoints[LockedPoints].position;
    }
}
