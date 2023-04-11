using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // For debugging
    public abstract string GetStateName();

    public abstract void EnterState(EnemyBase enemy);
    public abstract void UpdateState(EnemyBase enemy);
}
