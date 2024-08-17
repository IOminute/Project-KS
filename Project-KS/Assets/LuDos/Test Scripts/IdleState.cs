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

        Collider[] attackColliders = Physics.OverlapSphere(enemy.transform.position, enemy.chaseRange);
        foreach (var collider in attackColliders)
        {
            Vector3 targetPosition = collider.transform.position;
            targetPosition.y = 0f;

            Vector3 enemyPosition = enemy.transform.position;
            enemyPosition.y = 0f;

            if (Vector3.Distance(enemyPosition, targetPosition) <= enemy.attackRange)
            {
                enemy.ChangeState(new AttackState(enemy, collider.transform));
                break;
            }
            else if (Vector3.Distance(enemyPosition, targetPosition) > enemy.attackRange && Vector3.Distance(enemyPosition, targetPosition) <= enemy.chaseRange)
            {
                enemy.ChangeState(new ChaseState(enemy, collider.transform));
                break;
            }
            else
            {
                enemy.ChangeState(new MoveToCastleState(enemy));
                break;
            }
        }
    }

    public override void Exit()
    {

    }
}
