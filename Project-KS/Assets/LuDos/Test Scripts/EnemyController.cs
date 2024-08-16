using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float moveSpeed = 2f;
    public float attackCoolTime = 1f;
    public float chaseRange = 5f;
    public float attackRange = 1f;

    public bool isAttacking;

    public Animator animator;
    private StateMachine stateMachine;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stateMachine.Initialize(new MoveToCastleState(this));
        isAttacking = false;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        animator.SetTrigger("Run");

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // 회전 속도
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }


    public void ChaseTarget(Transform target)
    {
        MoveTo(target.position);
    }

    public virtual void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isAttacking = false;
        }
    }

    public void Die()
    {
        // stop

        animator.SetTrigger("Death");

        // Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
            }
        }
    }

    private void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }
}