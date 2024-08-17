using TMPro;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("IdleStart");
        enemy.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        if (enemy.IsDie) return;

        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("PlayerUnit");

        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var potentialTarget in potentialTargets)
        {
            Vector3 targetPosition = potentialTarget.transform.position;
            targetPosition.y = 0f;

            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.y = 0f;

            float distanceToTarget = Vector3.Distance(enemyPosition, targetPosition);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = potentialTarget;
            }
        }

        if (closestTarget != null)
        {
            Vector3 targetPosition = closestTarget.transform.position;
            targetPosition.y = 0f;

            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.y = 0f;

            float distanceToTarget = Vector3.Distance(enemyPosition, targetPosition);

            if (enemy.IsCastle || distanceToTarget <= enemy.attackRange || enemy.IsCastle)
            {
                enemy.ChangeState(new AttackState(enemy, closestTarget.transform));
            }
            else if (!enemy.IsAttacking && distanceToTarget > enemy.attackRange && distanceToTarget <= enemy.chaseRange)
            {
                enemy.ChangeState(new ChaseState(enemy, closestTarget.transform));
            }
            else
            {
                enemy.ChangeState(new MoveToCastleState(enemy));
            }
        }
        else
        {
            enemy.ChangeState(new MoveToCastleState(enemy));
        }
    }

    public override void Exit()
    {

    }
}
