using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private Transform target;

    public UnitAttackState(UnitController unit, Transform target) : base(unit)
    {
        this.target = target;
    }

    public override void Enter()
    {
        Debug.Log("AttackStart");
        unit.attackCoroutine = unit.StartCoroutine(AttackCoroutine());
    }

    public override void Update()
    {
        if (unit.IsDie) return;

        if (unit.IsSelected)
        {
            unit.ChangeState(new UnitSelectedState(unit));
        }

        if (!unit.IsAttacking && Vector3.Distance(unit.transform.position, target.position) > unit.attackRange + 0.5f)
        {
            Debug.Log("ChangeToChase");
            unit.ChangeState(new UnitChaseState(unit, target));
        }
    }

    public override void Exit()
    {
        Debug.Log("AttackFin");
        unit.StopAttack();
    }

    private IEnumerator AttackCoroutine()
    {
        unit.IsAttacking = true;

        unit.Attack(target.position);

        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(unit.attackCoolTime);

        unit.IsAttacking = false;

        unit.ChangeState(new UnitIdleState(unit));
    }
}
