using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSuspensionPointGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
