using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        // Idle 애니메이션 트리거
        enemy.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        // Idle 상태에서는 특정한 동작 없이 대기
        // 특정 조건이 충족되면 다른 상태로 전환 가능

        // 예: 일정 시간이 지나면 MoveToCastleState로 전환
        // enemy.ChangeState(new MoveToCastleState(enemy));
    }

    public override void Exit()
    {
        // IdleState에서 나갈 때 수행할 동작이 있다면 추가
    }
}