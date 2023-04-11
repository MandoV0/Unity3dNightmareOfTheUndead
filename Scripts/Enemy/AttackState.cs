using System.Collections;
using UnityEngine;

namespace Assets.GameProject.Scripts.Enemy
{
    public class AttackState : State
    {
        public override void EnterState(EnemyBase enemy)
        {
            enemy.Attack();
            enemy.SwitchState(new WaitState());
        }

        public override string GetStateName()
        {
            return "Attack-State";
        }

        public override void UpdateState(EnemyBase enemy)
        {
            
        }
    }
}