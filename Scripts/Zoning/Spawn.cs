using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Zoning
{
    public class Spawn : MonoBehaviour
    {
        public SpawnType spawnType;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (spawnType == SpawnType.PlayerSpawn)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.localPosition + Vector3.up, new Vector3(0.6f, 1.9f, 0.6f));
                Handles.Label(transform.localPosition + Vector3.up, "Player Spawn");
            }
            else
            {
                Gizmos.color = Color.red;
                Handles.Label(transform.localPosition, "Enemy Spawn");
            }

            Gizmos.DrawSphere(transform.position, 0.1f);
            
            Gizmos.DrawRay(transform.position, transform.forward);
#endif
        }
    }

    

    public enum SpawnType
    {
        Normal,
        Ground,
        PlayerSpawn
    }
}