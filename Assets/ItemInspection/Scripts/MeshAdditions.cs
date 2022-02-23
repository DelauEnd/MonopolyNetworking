using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ItemInspection.Scripts.Utils.Extensions;
using UnityEngine;

namespace Assets.ItemInspection.Scripts.Utils
{
    public class MeshAdditions
    {
        public static Bounds GetRenderBounds(GameObject objeto)
        {
            List<MeshRenderer> renderers = objeto.FindComponents<MeshRenderer>().ToList();
            if (renderers.Count == 0)
                return new Bounds(Vector3.zero, Vector3.zero);

            var minBound = GetMinBorder(renderers);
            var maxBound = GetMaxBorder(renderers);
            return new Bounds(minBound, maxBound);
        }

        private static Vector3 GetMaxBorder(List<MeshRenderer> renderers)
        {
            var maxX = renderers.Max(renderer => renderer.bounds.max.x);
            var maxY = renderers.Max(renderer => renderer.bounds.max.y);
            var maxZ = renderers.Max(renderer => renderer.bounds.max.z);

            return new Vector3(maxX, maxY, maxZ);
        }

        private static Vector3 GetMinBorder(List<MeshRenderer> renderers)
        {
            var minX = renderers.Max(renderer => renderer.bounds.max.x);
            var minY = renderers.Max(renderer => renderer.bounds.max.y);
            var minZ = renderers.Max(renderer => renderer.bounds.max.z);

            return new Vector3(minX, minY, minZ);
        }
    }
}
