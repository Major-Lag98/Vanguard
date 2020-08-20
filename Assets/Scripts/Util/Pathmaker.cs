using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pathmaker : MonoBehaviour
{

    public List<Vector3> points = new List<Vector3>();
    public bool Placing;

    

    public void OnDrawGizmos()
    {
        if (points.Count < 2)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(points[0], 0.1f);
        for (int i = 1; i < points.Count; i++)
        {
            var last = points[i - 1];
            var curr = points[i];

            Gizmos.color = Color.white;
            Gizmos.DrawLine(last, curr);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(curr, 0.1f);
        }
    }

  
}
