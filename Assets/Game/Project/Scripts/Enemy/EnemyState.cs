using Game.Project.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected Enemy owner;
    protected EnemyStateMachine machine;

    protected EnemyState(Enemy owner, EnemyStateMachine machine)
    {
        this.owner = owner;
        this.machine = machine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
