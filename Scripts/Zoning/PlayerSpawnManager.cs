using System;
using System.Collections.Generic;
using UnityEngine;
using Zoning;
using Random = UnityEngine.Random;

namespace _Scripts.Zoning
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        public static PlayerSpawnManager Instance;
        
        public List<Spawn> playerSpawns = new List<Spawn>();

        void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        public GameObject CreatePlayerSpawn()
        {
            GameObject spawn = new GameObject("Player Spawn");
            spawn.AddComponent<Spawn>();
            Spawn s = spawn.GetComponent<Spawn>();
            s.spawnType = SpawnType.PlayerSpawn;
            s.transform.SetParent(transform, false);
            playerSpawns.Add(s);
            return spawn;
        }

        public void ClearSpaws()
        {
            Debug.Log("Player Spawns Cleared");
            foreach (Spawn s in playerSpawns)
            {
                DestroyImmediate(s.gameObject);
            }

            playerSpawns.Clear();
        }
    
        public Transform GetRandomPlayerSpawn()
        {
            if(playerSpawns.Count > 0)
            {
                return playerSpawns[Random.Range(0, playerSpawns.Count)].transform;
            }
            Debug.LogError("No Player Spawns found!!!");
            return null;
        }
    }
}