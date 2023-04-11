using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zoning;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(Zone))]
public class ZoneCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Zone zone = (Zone)target;

        if(GUILayout.Button("Create Spawn"))
        {
            GameObject newSpawn = zone.CreateSpawn();
            Selection.activeGameObject = newSpawn;
            SceneView.FrameLastActiveSceneView();
        }

        if (GUILayout.Button("Clear Spawns"))
        {
            zone.ClearSpawns();
        }
        
        if (GUILayout.Button("Create Trigger"))
        {
            GameObject newTrigger = zone.CreateTrigger();
            Selection.activeGameObject = newTrigger;
            SceneView.FrameLastActiveSceneView();
        }
    }
}
