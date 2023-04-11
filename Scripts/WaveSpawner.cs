using Assets.GameProject.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zoning;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

namespace Wave_FPS_Mobile.Scripts
{
    public class WaveSpawner : MonoBehaviour
    {
        public static WaveSpawner Instance;

        [Header("Runtime Data (Readonly - Dont edit in Editor)")]
        [SerializeField] private RoundState roundState = RoundState.Wait;
        [SerializeField] private int round;
        [SerializeField] private int enemiesAlive;
        [SerializeField] private int enemiesKilled;
        [SerializeField] private int zombieHealth;
        [SerializeField] private int zombiesThisRound;
        [SerializeField] private int enemiesSpawned;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private float spawnInterval = 2;
        [SerializeField] private List<EnemyBase> currentEnemies = new List<EnemyBase>();

        [Header("Wave Spawner Settings")]
        [SerializeField] private float delayBetweenRounds = 10;
        [SerializeField] private EnemyPooler[] enemyPoolers;

        [Header("Wave Sound Settings")]
        [SerializeField] private AudioClip roundStart;
        [SerializeField] private AudioClip roundEnd;
        [SerializeField] private AudioSource roundSoundSource;

        private const int MaxEnemiesAtOnce = 24;
        private float _spawnTimer;

        [Header("Events")]
        [SerializeField] private UnityEvent OnNewRound;

        private void Awake()
        {
            Instance = this;
        }

        private void SpawnEnemy()
        {
            if (_spawnTimer != 0) return;
            if (enemiesAlive >= MaxEnemiesAtOnce) return;
            _spawnTimer = spawnInterval;

            List<Spawn> spawns = ZoneManager.Instance.GetActiveSpawns();
            Spawn spawn = spawns[Random.Range(0, spawns.Count)];
            Vector3 spawnPos = spawn.transform.position;
            Vector3 spawnRot = spawn.transform.rotation.eulerAngles;
            GameObject enemy = enemyPoolers[Random.Range(0, enemyPoolers.Length)].GetPooledObject();
            

            if (enemy != null)
            {
                enemy.transform.position = spawnPos;
                enemy.transform.rotation = Quaternion.Euler(spawnRot);
                enemy.SetActive(true);
            }
            else
            {
                return;
            }

            enemiesAlive++;
            enemiesSpawned++;

            // Walk, Run, Sprint ?
            // Initalize the enemy and add him to the list
            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            enemyBase.Initialize(zombieHealth, speedMultiplier);
            currentEnemies.Add(enemyBase);
        }

        public List<EnemyBase> GetEnemies() { return currentEnemies; }

        public void EnemyDied(EnemyBase enemy)
        {
            currentEnemies.Remove(enemy);
            enemiesAlive--;
            enemiesKilled++;
        }

        private void Start()
        {
            Invoke(nameof(NewRound), 5);
        }

        private void Update()
        {
            _spawnTimer = Mathf.MoveTowards(_spawnTimer, 0, Time.deltaTime);

            // If the round is in progress spawn zombies
            if (roundState == RoundState.Play)
            {
                if (enemiesKilled >= zombiesThisRound)
                {
                    EndRound();
                    return;
                }

                if (enemiesAlive < 24 && enemiesSpawned < zombiesThisRound)
                {
                    SpawnEnemy();
                }
            }
        }

        /// <summary>
        /// Calculates the base zombie health this round
        /// </summary>
        /// <param name="n"> The current round or wave </param>
        /// <returns> The base enemy health this round </returns>
        private int CalcZombieHealth(int n)
        {
            float health;
            if (n < 10)
            {
                health = 100 * n + 50;
            }
            else
            {
                health = 950 * Mathf.Pow(1.1f, n - 9);
            }

            return Mathf.RoundToInt(health);
        }

        /// <summary>
        /// Calculates how many Zombies will spawn this round
        /// </summary>
        /// <returns> The amount of zombies this round </returns>
        private int CalcZombieCount()
        {
            int zombieCount;
            int maxZombiesAtOnce = 24;

            /*
         For Multiplayer
        var playerCount = 1;
        if (playerCount > 1)
        {
            maxZombiesAtOnce = 24 + (playerCount * 6);
        }
        */

            if (round > 0 && round <= 5)
            {
                zombieCount = (int)Mathf.Floor((round * 0.2f) * maxZombiesAtOnce);
            }
            else
            {
                zombieCount = (int)Mathf.Floor((round * 0.15f) * maxZombiesAtOnce);
            }
            return zombieCount;
        }

        private void NewRound()
        {
            roundState = RoundState.Wait;

            // Call Event
            OnNewRound?.Invoke();

            roundSoundSource.Stop();
            roundSoundSource.PlayOneShot(roundStart);
            round++;

            enemiesAlive = 0;
            enemiesKilled = 0;
            enemiesSpawned = 0;

            // Reduce spawn interval by 5 Percent every round
            // Cap is at 0.8
            if (spawnInterval > 0.8)
            {
                spawnInterval *= 0.95f;
                spawnInterval = Mathf.Clamp(spawnInterval, 0.8f, 2);
            }

            if (speedMultiplier < 1.7f && round > 1)
            {
                speedMultiplier *= 1.04f;
            }

            zombieHealth = CalcZombieHealth(round);
            zombiesThisRound = CalcZombieCount();
            roundState = RoundState.Play;

            HUD.instance.SetRoundText(round.ToString());
        }

        private void EndRound()
        {
            roundState = RoundState.Wait;
            roundSoundSource.Stop();
            roundSoundSource.PlayOneShot(roundEnd);
            Invoke(nameof(NewRound), delayBetweenRounds);
        }

        public RoundState GetRoundState()
        {
            return roundState;
        }
    }

    [Serializable]
    public enum RoundState
    {
        Play,
        Wait,
        End
    }
}