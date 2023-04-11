using System.Collections;
using UnityEngine;

namespace Assets.GameProject.Scripts.Enemy
{
    public class WaitState : State
    {
        public override void EnterState(EnemyBase enemy)
        {
            
        }

        public override string GetStateName()
        {
            return "Wait-State";
        }

        public override void UpdateState(EnemyBase enemy)
        {
            Transform target = enemy.SearchForTarget();
            if (target != null) 
            {
                enemy.SwitchState(new ChaseState());
            }
        }
    }
}