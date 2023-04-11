using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation Events for the enemy
/// </summary>
public class EnemyAnimationEventHandler : MonoBehaviour
{
    private EnemyBase enemy;

    private void Awake()
    {
        // Cach the enemy
        enemy = transform.root.GetComponent<EnemyBase>();
    }

    /// <summary>
    /// Default Attack
    /// Attacks the Players in the attackRadius infront of the enemy
    /// </summary>
    public void Attack()
    {
        enemy.Attack();   
    }

    public void ActivateAttackRightHand()
    {

    }

    public void DeactivateAttackRightHand()
    {

    }
}