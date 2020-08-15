using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathmaker))]
public class PathmakerEditor : Editor
{
    private void OnSceneGUI()
    {
        if(Event.current.type == EventType.MouseUp && Event.current.button == 1 && target is Pathmaker maker && maker.Placing)
        {
            var p = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            var x = p.x > 0 ? ((int)p.x) + 0.5f : ((int)p.x) - 0.5f;
            var y = p.y > 0 ? ((int)p.y) + 0.5f : ((int)p.y) - 0.5f;
            maker.points.Add(new Vector3(x, y, 0));
        }
    }
}
