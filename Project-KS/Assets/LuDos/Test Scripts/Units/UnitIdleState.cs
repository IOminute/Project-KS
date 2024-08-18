using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public UnitIdleState(UnitController unit) : base(unit) { }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (unit.IsDie) return;

        if (unit.IsSelected)
        {
            unit.ChangeState(new UnitSelectedState(unit));
        }

        Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, unit.chaseRange);

        Collider closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Spear") || hitCollider.CompareTag("Mage") || hitCollider.CompareTag("Hammer") || 
                hitCollider.CompareTag("Good_Knight") || hitCollider.CompareTag("Bad_Knight") || hitCollider.CompareTag("Cavarly_Sword") || 
                hitCollider.CompareTag("Cavarly_Spear") || hitCollider.CompareTag("Cavarly_Mage") || hitCollider.CompareTag("Cavarly_Archer") || hitCollider.CompareTag("Archer")) // Àû±º tag
            {
                Vector3 targetPosition = hitCollider.transform.position;
                targetPosition.y = 0f;

                Vector3 enemyPosition = unit.transform.position;
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
            unit.ChangeState(new UnitChaseState(unit, closestTarget.transform));
        }

    }
}
