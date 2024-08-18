using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChaseState : UnitBaseState
{
    private Transform target;

    public UnitChaseState(UnitController unit, Transform target) : base(unit)
    {
        this.target = target;
    }

    public override void Enter()
    {
        Debug.Log("ChaseStart");
    }

    public override void Update()
    {
        if (unit.IsDie) return;

        if (unit.IsSelected)
        {
            unit.ChangeState(new UnitSelectedState(unit));
        }

        unit.ChaseTarget(target);

        Vector3 targetPosition = target.position;
        targetPosition.y = 0f;

        Vector3 enemyPosition = unit.transform.position;
        enemyPosition.y = 0f;

        if (Vector3.Distance(unit.transform.position, target.position) <= unit.attackRange)
        {
            unit.ChangeState(new UnitAttackState(unit, target));
        }
        else if (Vector3.Distance(unit.transform.position, target.position) > unit.chaseRange)
        {
            unit.ChangeState(new UnitIdleState(unit));
        }
    }
}
