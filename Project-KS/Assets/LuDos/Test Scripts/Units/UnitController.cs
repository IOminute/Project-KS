using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
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
    public bool IsSelected; // 마우스 우클릭 시 on, 우클릭 해제할 수 있다면 해제 시 off
    [HideInInspector]
    public Vector3 ClickedPosition; // 마우스 우클릭한 world 상의 position

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody Rb;

    private UnitStateMachine stateMachine;
    [HideInInspector]
    public Coroutine attackCoroutine;

    public GameObject Projectile;
    public Transform WeaponPosition;
    public Collider weaponCollider;

    private void Awake()
    {
        stateMachine = new UnitStateMachine();
        animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        stateMachine.Initialize(new UnitIdleState(this));
        IsAttacking = false;
        IsDie = false;
        DisabledWeaponCollider();
    }

    private void Update()
    {
        stateMachine.Update();

        // Selected 되었는지 확인
        if (IsSelected)
        {
            // ClickedPosition = ; // 우클릭 포지션 업데이트
        }
    }

    public void EnabledWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }
    public void DisabledWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f); // 회전 속도
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

    public virtual void Attack(Transform targetTransform)
    {
        Stop();

        Vector3 targetPosition = targetTransform.position;
        targetPosition.y = 0f;

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }

        //animator.SetTrigger("Attack");
        //EnabledWeaponCollider();

        Vector3 tmp = targetTransform.position;
        tmp.y += 2.5f;

        if (WeaponPosition == null)
        {
            WeaponPosition = gameObject.transform;
        }
        StartCoroutine(FireProjectileWithDelay(targetTransform, (tmp - WeaponPosition.position).normalized));
    }
    private IEnumerator FireProjectileWithDelay(Transform targetTransform, Vector3 direction)
    {
        yield return new WaitForSeconds(0.7f);

        if (Projectile != null)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(direction);
            GameObject projectile = Instantiate(Projectile, WeaponPosition.position + direction, projectileRotation);

            PlayerWeapon_Projectile projectileScript = projectile.GetComponent<PlayerWeapon_Projectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(targetTransform, gameObject);
            }
        }
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

        animator.SetTrigger("Death");

        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon"))
        {
            EnemyWeapon weapon = other.GetComponent<EnemyWeapon>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
            }
        }
        else if (other.CompareTag("EnemyWeapon_Projectile"))
        {
            EnemyWeapon_Projectile weapon = other.GetComponent<EnemyWeapon_Projectile>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                Destroy(other.gameObject);
            }
        }
    }

    private void TakeDamage(float damageAmount)
    {
        if (!IsDie)
        {
            health -= damageAmount;
            // Debug.Log("Unit: " + health);

            if (health <= 0)
            {
                health = 0;
                IsDie = true;
                Die();
            }
        }
    }

    public void ChangeState(UnitBaseState newState)
    {
        stateMachine.ChangeState(newState);
    }
}
