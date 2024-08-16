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

        // Ÿ���� ���� ���� ���� ������ AttackState�� ��ȯ
        if (Vector3.Distance(enemy.transform.position, target.position) <= enemy.attackRange)
        {
            Debug.Log("AttakcState");
            enemy.ChangeState(new AttackState(enemy, target));
        }
        // Ÿ���� ChaseRange�� ������� MoveToCastleState�� ��ȯ
        else if (Vector3.Distance(enemy.transform.position, target.position) > enemy.chaseRange)
        {
            enemy.ChangeState(new MoveToCastleState(enemy));
        }
    }
}