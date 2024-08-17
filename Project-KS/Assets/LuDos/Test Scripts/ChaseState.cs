using UnityEngine;

public class ChaseState : BaseState
{
    private Transform target;

    public ChaseState(EnemyController enemy, Transform target) : base(enemy)
    {
        this.target = target;
    }

    public override void Enter()
    {
        Debug.Log("ChaseStart");
    }

    public override void Update()
    {
        if (enemy.IsDie) return;

        enemy.ChaseTarget(target);

        Vector3 targetPosition = target.position;
        targetPosition.y = 0f;

        Vector3 enemyPosition = enemy.transform.position;
        enemyPosition.y = 0f;

        if (Vector3.Distance(enemy.transform.position, target.position) <= enemy.attackRange)
        {
            enemy.ChangeState(new AttackState(enemy, target));
        }
        else if (Vector3.Distance(enemy.transform.position, target.position) > enemy.chaseRange)
        {
            enemy.ChangeState(new MoveToCastleState(enemy));
        }
    }
}