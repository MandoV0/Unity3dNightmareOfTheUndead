using Assets.GameProject.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wave_FPS_Mobile.Scripts;

public class PU_Nuke : PowerUpBase
{
    public override void OnPlayerTakePowerUp(PlayerStats player)
    {
        if (WaveSpawner.Instance)
        {
            List<EnemyBase> enemies = WaveSpawner.Instance.GetEnemies();

            foreach (EnemyBase enemy in enemies.ToList()) 
            {
                enemy.Die();
            }
        }
    }
}