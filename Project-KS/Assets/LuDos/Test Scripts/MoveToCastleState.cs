using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveToCastleState : BaseState
{
    public MoveToCastleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
       // Debug.Log("MopveToCastleStart");
    }

    public override void Update()
    {
        if (enemy.IsDie) return;

        Transform castleTransform = enemy.Castle.transform;

        if (enemy.IsRangeShort || enemy.IsRangeLong)
        {
            enemy.ChangeState(new AttackState(enemy, castleTransform));
            return;
        }

        enemy.MoveTo(castleTransform.position);

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

        if (closestTarget != null && Vector3.Distance(enemy.transform.position, closestTarget.transform.position) <= enemy.chaseRange)
        {
            enemy.ChangeState(new ChaseState(enemy, closestTarget.transform));
        }

    }
}