using UnityEngine;

public class ChaseState : BaseState
{
    private Transform target;

    public ChaseState(EnemyController enemy, Transform target) : base(enemy)
    {
        this.target = target;
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        enemy.ChaseTarget(target);

        // 타겟이 공격 범위 내에 있으면 AttackState로 전환
        if (Vector3.Distance(enemy.transform.position, target.position) <= enemy.attackRange)
        {
            Debug.Log("AttakcState");
            enemy.ChangeState(new AttackState(enemy, target));
        }
        // 타겟이 ChaseRange를 벗어났으면 MoveToCastleState로 전환
        else if (Vector3.Distance(enemy.transform.position, target.position) > enemy.chaseRange)
        {
            enemy.ChangeState(new MoveToCastleState(enemy));
        }
    }
}