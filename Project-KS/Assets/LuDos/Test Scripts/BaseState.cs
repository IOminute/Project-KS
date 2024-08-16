using UnityEngine;

public abstract class BaseState
{
    protected EnemyController enemy;

    public BaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}