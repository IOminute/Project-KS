using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class AttackState : BaseState
{
    private Transform target;

    public AttackState(EnemyController enemy, Transform target) : base(enemy)
    {
        this.target = target;
    }

    public override void Enter()
    {
        enemy.attackCoroutine = enemy.StartCoroutine(AttackCoroutine());
    }

    public override void Update()
    {
        if (Vector3.Distance(enemy.transform.position, target.position) > enemy.attackRange + 1)
        {
            enemy.ChangeState(new ChaseState(enemy, target));
        }
    }

    public override void Exit()
    {
        enemy.StopAttack();
    }

    private IEnumerator AttackCoroutine()
    {
        enemy.isAttacking = true;

        enemy.Attack();
        
        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(enemy.attackCoolTime);

        enemy.isAttacking = false;

        enemy.ChangeState(new IdleState(enemy));
    }
}