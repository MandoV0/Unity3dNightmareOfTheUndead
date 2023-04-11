using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Zoning
{
    public class Zone : MonoBehaviour
    {
        public List<Spawn> spawnList = new List<Spawn>();
        public bool isActive;
        public bool hasBeenEnterd;

        public GameObject CreateSpawn()
        {
            GameObject spawn = new GameObject("Spawn");
            spawn.AddComponent<Spawn>();
            Spawn s = spawn.GetComponent<Spawn>();
            spawnList.Add(s);
            spawn.transform.SetParent(this.transform, false);
            return spawn;
        }

        public void ClearSpawns()
        {
            foreach (Spawn s in spawnList)
            {
                DestroyImmediate(s.gameObject);
            }

            spawnList.Clear();
        }

        public GameObject CreateTrigger()
        {
            GameObject triggerGameObject = new GameObject($"{gameObject.name} Trigger");
            
            ZoneTrigger zoneTrigger = triggerGameObject.AddComponent<ZoneTrigger>();
            BoxCollider boxCollider = triggerGameObject.AddComponent<BoxCollider>();

            boxCollider.isTrigger = true;
            
            zoneTrigger.zone = this;
            
            triggerGameObject.transform.SetParent(this.transform, false);
            triggerGameObject.layer = 2; // Change Layer to Ignore Raycast so that triggers dont block bullets 
            
            return triggerGameObject;
        }
    }
}
