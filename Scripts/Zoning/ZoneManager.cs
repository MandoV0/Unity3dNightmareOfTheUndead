using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Zoning
{
    public class ZoneManager : MonoBehaviour
    {
        public static ZoneManager Instance;

        [SerializeField] private List<Zone> zones = new List<Zone>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void CreateZone()
        {
            GameObject zone = new GameObject("Zone");
            zone.AddComponent<Zone>();
            Zone z = zone.GetComponent<Zone>();
            zone.transform.SetParent(transform, false);
            zones.Add(z);
        }

        public void ClearZones()
        {
            foreach (Zone z in zones)
            {
                DestroyImmediate(z.gameObject);
            }

            zones.Clear();
        }

        public List<Spawn> GetActiveSpawns()
        {
            List<Spawn> activeSpawns = new List<Spawn>();
            
            foreach(Zone z in zones)
            {
                if(z.isActive)
                {
                    activeSpawns.AddRange(z.spawnList);
                }
            }

            if (activeSpawns.Count == 0)
            {
                Debug.LogWarning("No Active Spawns have been found! Returning all activated Zones");
                
                foreach(Zone z in zones)
                {
                    if(z.hasBeenEnterd)
                    {
                        activeSpawns.AddRange(z.spawnList);
                    }
                }
            }

            if (activeSpawns.Count == 0)
            {
                Debug.LogWarning("No Active Spawns have been found! Returning all Zones");

                foreach (Zone z in zones)
                {
                    activeSpawns.AddRange(z.spawnList);
                }
            }

            return activeSpawns;
        }
    }
}