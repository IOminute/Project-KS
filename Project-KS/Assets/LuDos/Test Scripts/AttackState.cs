using System.Collections;
using System.ComponentModel;
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
        Debug.Log("AttackStart");
        enemy.attackCoroutine = enemy.StartCoroutine(AttackCoroutine());
    }

    public override void Update()
    {
        if (enemy.IsDie) return;

        if (Vector3.Distance(enemy.transform.position, target.position) > enemy.attackRange + 1)
        {
            Debug.Log("ChangeToChase");
            //enemy.ChangeState(new ChaseState(enemy, target));
        }
    }

    public override void Exit()
    {
        Debug.Log("AttackFin");
        enemy.StopAttack();
    }

    private IEnumerator AttackCoroutine()
    {
        enemy.isAttacking = true;

        enemy.Attack();
        
        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(enemy.attackCoolTime);

        enemy.isAttacking = false;

        Debug.Log("Real?");

        enemy.ChangeState(new IdleState(enemy));
    }
}