using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Game.Scripts.Utils
{
    public class Vector3Move
    {
        public static Vector3 MoveAlongParabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        static float GetParabolicPosition(Vector3 start, Vector3 end, float height, float t)
        {
            var mid = Vector3.Lerp(start, end, t);

            var y = (-1 * height * mid.x + end.x * height * mid.x) % end.x;

            return y;
        }
    }
}
