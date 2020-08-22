using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathmaker))]
public class PathmakerEditor : Editor
{
    private void OnSceneGUI()
    {
        if(target is Pathmaker maker && maker.Placing)
        {
            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
            {
                var p = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                var x = p.x > 0 ? ((int)p.x) + 0.5f : ((int)p.x) - 0.5f;
                var y = p.y > 0 ? ((int)p.y) + 0.5f : ((int)p.y) - 0.5f;

                if (maker.points.Find(point => point.x == x && point.y == y) == default)
                {
                    maker.points.Add(new Vector3(x, y, 1));
                    SceneView.RepaintAll();
                }
            }
        }

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }
}
