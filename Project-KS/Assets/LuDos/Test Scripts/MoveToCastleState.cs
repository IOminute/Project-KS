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

        enemy.MoveTo(castleTransform.position);

        if (enemy.IsRangeShort || enemy.IsRangeLong)
        {
            enemy.ChangeState(new AttackState(enemy, castleTransform));
        }

        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, enemy.chaseRange);

        Collider closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("PlayerUnit")) // ¾Æ±º tag
            {
                Vector3 targetPosition = hitCollider.transform.position;
                targetPosition.y = 0f;

                Vector3 enemyPosition = enemy.transform.position;
                enemyPosition.y = 0f;

                float distanceToTarget = Vector3.Distance(enemyPosition, targetPosition);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = hitCollider;
                }
            }
        }

        if (closestTarget != null)
        {
            enemy.ChangeState(new ChaseState(enemy, closestTarget.transform));
        }

    }
}