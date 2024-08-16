using UnityEngine;

public class MoveToCastleState : BaseState
{
    public MoveToCastleState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {

    }

    public override void Update()
    {
        Vector3 castlePosition = Vector3.zero; // [�ӽÿ�] ���� ��ġ
        enemy.MoveTo(castlePosition);

        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, enemy.chaseRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("PlayerUnit")) // �Ʊ� tag
            {
                enemy.ChangeState(new ChaseState(enemy, hitCollider.transform));
                break;
            }
        }
    }
}