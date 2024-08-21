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
            unit.ChangeState(new UnitChaseState(unit, target));
        }
    }

    public override void Exit()
    {
        unit.StopAttack();
    }

    private IEnumerator AttackCoroutine()
    {
        unit.IsAttacking = true;

        unit.Attack(target);

        unit.animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.6f);
        unit.EnabledWeaponCollider();

        yield return new WaitForSeconds(0.4f);

        unit.DisabledWeaponCollider();

        yield return new WaitForSeconds(unit.attackCoolTime);

        unit.IsAttacking = false;

        unit.ChangeState(new UnitIdleState(unit));
    }
}
