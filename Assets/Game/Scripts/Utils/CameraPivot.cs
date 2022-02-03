using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    void Update()
    {
        if (transform.localPosition != Vector3.zero)
            transform.localPosition = Vector3.zero;

        if (transform.position != Vector3.zero)
            transform.position = Vector3.zero;
    }
}
