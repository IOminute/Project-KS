using UnityEngine;

public class MoveToCastleState : BaseState
{
    public MoveToCastleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        // enemy.animator.SetTrigger("Idle");
    }

    public override void Update()
    {
        Vector3 castlePosition = Vector3.zero; // [임시완] 성의 위치
        enemy.MoveTo(castlePosition);

        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, enemy.chaseRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("PlayerUnit") || hitCollider.CompareTag("Castle")) // 아군 tag
            {
                enemy.ChangeState(new ChaseState(enemy, hitCollider.transform));
                break;
            }
        }
    }
}