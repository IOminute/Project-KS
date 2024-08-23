using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TreeEditor;

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
    public bool IsRangeShort;
    [HideInInspector]
    public bool IsRangeLong;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody Rb;

    [HideInInspector]
    public GameObject Castle;

    private StateMachine stateMachine;
    [HideInInspector]
    public Coroutine attackCoroutine;

    int soulIndex;
    public GameObject soulPrefab;
    public GameObject Projectile;
    public Transform WeaponPosition;
    public Collider weaponCollider;

    private void Awake()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();
        Rb= GetComponent<Rigidbody>();
        AssignSoulIndexBasedOnTag();
        Castle = GameObject.FindWithTag("Castle");
    }

    private void AssignSoulIndexBasedOnTag()
    {
        switch (gameObject.tag)
        {
            case "Archer":
                soulIndex = 0;
                break;
            case "Bad_Knight":
                soulIndex = 1;
                break;
            case "Cavarly_Archer":
                soulIndex = 2;
                break;
            case "Cavarly_Mage":
                soulIndex = 3;
                break;
            case "Cavarly_Spear":
                soulIndex = 4;
                break;
            case "Cavarly_Sword":
                soulIndex = 5;
                break;
            case "Hammer":
                soulIndex = 6;
                break;
            case "Mage":
                soulIndex = 7;
                break;
            case "Spear":
                soulIndex = 8;
                break;
            case "Good_Knight":
                soulIndex = 9;
                break;
            default:
                soulIndex = -1; // �� �� ���� �±״� -1�� ����
                break;
        }
    }

    private void Start()
    {
        stateMachine.Initialize(new MoveToCastleState(this));
        IsAttacking = false;
        IsDie = false;
        IsRangeShort = false;
        IsRangeLong = false;
        DisabledWeaponCollider();
    }

    private void Update()
    {
        stateMachine.Update();
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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f); // ȸ�� �ӵ�
        }

        if (!IsRangeShort)
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

        if (soulIndex == 0 || soulIndex == 2 || soulIndex == 3 || soulIndex == 7)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(direction);
            GameObject projectile = Instantiate(Projectile, WeaponPosition.position + direction, projectileRotation);

            EnemyWeapon_Projectile projectileScript = projectile.GetComponent<EnemyWeapon_Projectile>();

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

        //SceneManagement.Instance.enemies.Remove(gameObject); // �� ����Ʈ���� �ڱ� �ڽ� ����

        DropSoul();

        animator.SetTrigger("Death");

        Necromancer.ManageKindredPoint(1);

        Destroy(gameObject, 2.0f);
    }

    private void DropSoul()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = 2.0f;

        GameObject soul = Instantiate(soulPrefab, spawnPosition, Quaternion.identity);

        Soul soulComponent = soul.GetComponent<Soul>();
        soulComponent.soulIndex = soulIndex;

        Necromancer.AddSpirit(soul); // �ҿ� �߰�
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
        else if (other.CompareTag("PlayerWeapon_Projectile"))
        {
            PlayerWeapon_Projectile weapon = other.GetComponent<PlayerWeapon_Projectile>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("KnightWeapon"))
        {
            KnightWeapon weapon = other.GetComponent<KnightWeapon>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage);
            }
        }
        else if (other.CompareTag("Range_Long"))
        {
            if (soulIndex == 0 || soulIndex == 2 || soulIndex == 3 || soulIndex == 7)
            {
                IsRangeLong = true;
                transform.position = transform.position;
            }
        }
        else if (other.CompareTag("Range_Short"))
        {
            IsRangeShort = true;
        }
    }

    private void TakeDamage(float damageAmount)
    {
        if (!IsDie)
        {
            health -= damageAmount;
            // Debug.Log("Enemy: " + health);

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