using Assets.GameProject.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    /// <summary>
    /// Deals damage to the enemy
    /// </summary>
    /// <param name="damage"> The amount of damage taken </param>
    /// <param name="hitLayer"> The layer we hit </param>
    /// <param name="damageCauserLook"> The look direction of the damage causer </param>
    public abstract void TakeDamage(int damage, PlayerStats damageCauser, Vector3 damageCauserLook, HitLayer hitLayer, RaycastHit hit);

    /// <summary>
    /// Returns if the enemy is Alive or not
    /// </summary>
    /// <returns>Alive</returns>
    public abstract bool IsAlive();

    /// <summary>
    /// Sets the variables given by the WaveSpawner and Starts the enemy AI (Usefull for Object Pooling)
    /// </summary>
    /// <param name="speedMultiplier"></param>
    /// <param name="baseSpeed"></param>
    public abstract void Initialize(int baseHealth, float speedMultiplier);

    /// <summary>
    /// Sets the enemy death and stops AI 
    /// </summary>
    public abstract void Die();

    public abstract void SwitchState(State state);

    /// <summary>
    /// Returns a valid target
    /// </summary>
    /// <returns> Transform of the target </returns>
    public abstract Transform SearchForTarget();

    public abstract void Attack();

    public abstract void FaceTarget(Transform target);

    public float DistanceToTarget(Transform target)
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public abstract NavMeshAgent GetNavMeshAgent();

    public abstract float GetAttackingDistance();
}
