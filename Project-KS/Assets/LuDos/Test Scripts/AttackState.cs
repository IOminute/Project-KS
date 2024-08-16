using System.Collections;
using System.Threading;
using UnityEngine;

public class AttackState : BaseState
{
    private Transform target;
    private float lastAttackTime;

    public AttackState(EnemyController enemy, Transform target) : base(enemy)
    {
        this.target = target;
    }

    public override void Enter()
    {
        enemy.StartCoroutine(AttackCoroutine());
    }

    public override void Update()
    {
        if (Vector3.Distance(enemy.transform.position, target.position) > enemy.attackRange)
        {
            Debug.Log("Chase");
            enemy.ChangeState(new ChaseState(enemy, target));
        }
    }

    public override void Exit()
    {
        Debug.Log("Fin");
        // enemy.StopAttack();
    }

    private IEnumerator AttackCoroutine()
    {
        enemy.isAttacking = true;

        enemy.Attack();
        
        yield return new WaitForSeconds(1.0f);

        yield return new WaitForSeconds(enemy.attackCoolTime);

        enemy.isAttacking = false;

        enemy.ChangeState(new ChaseState(enemy, target));
    }
}