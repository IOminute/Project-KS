using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        // Idle �ִϸ��̼� Ʈ����
        enemy.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        // Idle ���¿����� Ư���� ���� ���� ���
        // Ư�� ������ �����Ǹ� �ٸ� ���·� ��ȯ ����

        // ��: ���� �ð��� ������ MoveToCastleState�� ��ȯ
        // enemy.ChangeState(new MoveToCastleState(enemy));
    }

    public override void Exit()
    {
        // IdleState���� ���� �� ������ ������ �ִٸ� �߰�
    }
}