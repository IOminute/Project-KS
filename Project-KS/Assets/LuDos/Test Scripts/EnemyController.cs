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

    [HideInInspector]
    public bool IsAttacking;
    [HideInInspector]
    public bool IsDie;
    [HideInInspector]
    public bool IsCastle;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody Rb;

    public GameObject Castle;

    private StateMachine stateMachine;
    [HideInInspector]
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
        IsAttacking = false;
        IsDie = false;
        IsCastle = false;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f); // ȸ�� �ӵ�
        }

        if (!IsCastle)
        {
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
            newPosition.y = 0f;
            transform.position = newPosition;
        }

    }

    public void ChaseTarget(Transform target)
    {
        MoveTo(target.position);
    }

    public void Stop()
    {
        Rb.velocity = Vector3.zero;
        transform.position = transform.position;
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
            IsAttacking = false;
        }
    }

    public void Die()
    {
        Stop();

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        //SceneManagement.Instance.enemies.Remove(gameObject); // �� ����Ʈ���� �ڱ� �ڽ� ����
        //Necromancer.AddSpirit(gameObject); // �� ��ȥ ����Ʈ�� �ڱ� �ڽ� �߰�

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
        else if (other.CompareTag("Castle"))
        {
            Debug.Log("Castle");
            IsCastle = true;
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