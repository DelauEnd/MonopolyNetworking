using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] public string Name;

    [Header("Marker")]
    [ColorUsage(false)]
    [SerializeField] private Color DrawColor;
    [SerializeField] private float MarkerSize = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = DrawColor;
        Gizmos.DrawSphere(transform.position, MarkerSize);
    }
}
