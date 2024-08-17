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
    public bool IsDie;

    public Animator animator;
    public Rigidbody Rb;

    private StateMachine stateMachine;
    public Coroutine attackCoroutine;

    private void Awake()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();
        Rb= GetComponent<Rigidbody>();
    }

    private void Start()
    {
        stateMachine.Initialize(new MoveToCastleState(this));
        isAttacking = false;
        IsDie = false;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        // Debug.Log("MoveTo");

        animator.SetTrigger("Run");

        targetPosition.y = 0f;

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // 회전 속도
        }

        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        newPosition.y = 0f;
        transform.position = newPosition;
    }

    public void ChaseTarget(Transform target)
    {
        MoveTo(target.position);
    }

    public void Stop()
    {
        Rb.velocity = Vector3.zero;
    }

    public virtual void Attack()
    {
        Stop();
        animator.SetTrigger("Attack");
    }

    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            animator.SetTrigger("Idle");
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isAttacking = false;
        }
    }

    public void Die()
    {
        Stop();

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        SceneManagement.Instance.enemies.Remove(gameObject); // 적 리스트에서 자기 자신 제거
        Necromancer.AddSpirit(gameObject); // 적 영혼 리스트에 자기 자신 추가

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
        if (!IsDie)
        {
            health -= damageAmount;

            if (health <= 0)
            {
                health = 0;
                IsDie = true;
                Die();
            }
        }
    }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }
}