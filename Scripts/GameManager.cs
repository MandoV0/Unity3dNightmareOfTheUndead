using _Scripts.Zoning;
using Assets.GameProject.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Prefabs")]
    [SerializeField] private PlayerStats playerPrefab;

    [Header("Runtime")]
    [SerializeField] private List<PlayerStats> players = new List<PlayerStats>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// Spawns a player at a random player spawn of the player spawn manager
    /// </summary>
    /// <returns> The created Player </returns>
    public PlayerStats SpawnPlayer()
    {
        // Check if we have a playerPrefab
        if (playerPrefab == null)
        {
            Debug.LogError("The GameManager has no playerPrefab assigned. Cant spawn player");
            return null;
        }

        if (PlayerSpawnManager.Instance)
        {
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;

            // Get a playerSpawn
            if (PlayerSpawnManager.Instance)
            {
                Transform playerSpawn = PlayerSpawnManager.Instance.GetRandomPlayerSpawn();
                spawnPosition = playerSpawn.position;
                spawnRotation = playerSpawn.rotation;
            }
            
            // Instantiate the player at the playerSpawn
            PlayerStats tmpPlayer = Instantiate(playerPrefab, spawnPosition, spawnRotation);

            players.Add(tmpPlayer);
            return tmpPlayer;
        }
        return null;
    }

    /// <summary>
    /// Return the player in the list at the index of the value id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> The Player </returns>
    public PlayerStats GetPlayerWithId(int id)
    {
        return players[id];
    }
}