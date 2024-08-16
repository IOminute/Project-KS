using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        Collider[] attackColliders = Physics.OverlapSphere(enemy.transform.position, enemy.attackRange);
        foreach (var collider in attackColliders)
        {
            Vector3 targetPosition = collider.transform.position;
            targetPosition.y = 0f;

            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.y = 0f;

            if (Vector3.Distance(enemyPosition, targetPosition) <= enemy.attackRange)
            {
                enemy.ChangeState(new AttackState(enemy, collider.transform));
                return;
            }
        }

        Collider[] chaseColliders = Physics.OverlapSphere(enemy.transform.position, enemy.chaseRange);
        foreach (var collider in chaseColliders)
        {
            Vector3 targetPosition = collider.transform.position;
            targetPosition.y = 0f;

            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.y = 0f;

            if (Vector3.Distance(enemyPosition, targetPosition) <= enemy.chaseRange)
            {
                enemy.ChangeState(new ChaseState(enemy, collider.transform));
                return;
            }
        }
        enemy.ChangeState(new MoveToCastleState(enemy));
    }

    public override void Exit()
    {

    }
}
