using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ItemInspection.Scripts.Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<T> FindComponentsInChild<T>(this GameObject parent) where T : Component
        {
            if (parent.transform.childCount == 0)
            {
                return null;
            }

            List<T> result = new List<T>();
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                var objects = child.GetComponents<T>();
                if (objects != null)
                    result.AddRange(objects);

                var childObjects = FindComponentsInChild<T>(child.gameObject);
                if(childObjects!=null)
                    result.AddRange(childObjects);
            }

            return result;
        }

        public static void ChangeObjectLayer(this GameObject parent, int layer)
        {
            parent.layer = layer;

            if (parent.transform.childCount == 0)
                return;

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                ChangeObjectLayer(child.gameObject, layer);
            }
        }

        public static IEnumerable<T> FindComponents<T>(this GameObject gameObj) where T : Component
        {
            List<T> result = new List<T>();

            var objects = gameObj.GetComponents<T>();
            if (objects != null)
                result.AddRange(objects);

            if (gameObj.transform.childCount == 0)
                return result;
            for (int i = 0; i < gameObj.transform.childCount; i++)
            {
                var child = gameObj.transform.GetChild(i);
                objects = child.GetComponents<T>();
                if (objects != null)
                    result.AddRange(objects);

                var childObjects = FindComponentsInChild<T>(child.gameObject);
                if (childObjects != null)
                    result.AddRange(childObjects);
            }

            return result;
        }
    }
}
