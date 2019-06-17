using UnityEngine;
using System.Collections;

using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(CalibrateTooltip))]
public class CalibrateTooltipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CalibrateTooltip myScript = (CalibrateTooltip)target;
        if (GUILayout.Button("Reset"))
        {
            myScript.Clear();
        }
    }
}
#endif
