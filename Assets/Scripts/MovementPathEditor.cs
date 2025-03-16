using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovementPath))]
public class MovementPathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.color = Color.green;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 120f,
            fixedHeight = 30f
        };

        if (GUILayout.Button("Add Path Node", buttonStyle))
        {
            ((MovementPath)target).AddPathNode();
        }

        GUI.color = Color.white;
        DrawDefaultInspector();
    }
}
