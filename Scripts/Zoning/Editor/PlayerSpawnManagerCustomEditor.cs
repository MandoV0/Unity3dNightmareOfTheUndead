using UnityEditor;
using UnityEngine;

namespace _Scripts.Zoning.Editor
{
    [CustomEditor(typeof(PlayerSpawnManager))]
    public class PlayerSpawnManagerCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerSpawnManager playerSpawnManager = (PlayerSpawnManager)target;

            if (GUILayout.Button("Create Spawn"))
            {
                GameObject newSpawn = playerSpawnManager.CreatePlayerSpawn();
                Selection.activeGameObject = newSpawn;
                SceneView.FrameLastActiveSceneView();
            }

            if(GUILayout.Button("Clear Spawns"))
            {
                playerSpawnManager.ClearSpaws();
            }
        }
    }
}