using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    private int currPlacingType;

    private void OnSceneGUI()
    {
        if (target is Grid grid) // Pattern match
        {
            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0) // If we're dragging the left mouse button
            {
                var p = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin; // Get screen to world
                var x = p.x > 0 ? ((int)p.x) + 0.5f : ((int)p.x) - 0.5f; // Adjust
                var y = p.y > 0 ? ((int)p.y) + 0.5f : ((int)p.y) - 0.5f; // Adjust

                grid.SetValue(new Vector3(x, y), currPlacingType); // Set the type on the grid
                SceneView.RepaintAll();
            }
        }

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        //LevelScript myTarget = (LevelScript)target;

        //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

        if(GUILayout.Button("None")){
            currPlacingType = 0;
        }else if (GUILayout.Button("Tower"))
        {
            currPlacingType = 1;
        }
        else if (GUILayout.Button("Crops"))
        {
            currPlacingType = 2;
        }
        else if (GUILayout.Button("Blocked"))
        {
            currPlacingType = 3;
        }

        if (GUILayout.Button("Generate"))
        {
            ((Grid)target).GenerateGrid();
            EditorUtility.SetDirty((Grid)target);
        }

        //generate grid here
    }
}
