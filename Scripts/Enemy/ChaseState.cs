using Assets.GameProject.Scripts;
using Assets.GameProject.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private Transform target;
    private float setTargetInterval = 0.3f;
    private float setTargetTimer = 0;

    public override void EnterState(EnemyBase enemy)
    {
        target = enemy.SearchForTarget();
    }

    public override string GetStateName()
    {
        return "Chase-State";
    }

    public override void UpdateState(EnemyBase enemy)
    {
        setTargetTimer = Mathf.MoveTowards(setTargetTimer, 0, Time.deltaTime);

        if (target != null) 
        {
            enemy.FaceTarget(target);

            if (enemy.DistanceToTarget(target) <= enemy.GetAttackingDistance())
            {
                enemy.SwitchState(new AttackState());
            }
        }

        if (setTargetTimer == 0)
        {
            setTargetTimer = setTargetInterval;
            target = enemy.SearchForTarget();

            if (target != null)
            {
                enemy.GetNavMeshAgent().SetDestination(target.position);
            }
            else
            {
                enemy.SwitchState(new WaitState());
            }
        }
    }  
}