using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zoning;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(ZoneManager))]
public class ZoneManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ZoneManager zoneManager = (ZoneManager)target;

        if(GUILayout.Button("Create Zone"))
        {
            zoneManager.CreateZone();
        }

        if (GUILayout.Button("Clear Zones"))
        {
            zoneManager.ClearZones();
        }
    }
}